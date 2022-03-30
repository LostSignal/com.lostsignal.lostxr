//-----------------------------------------------------------------------
// <copyright file="HavenSocketTarget.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost.Haven
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.XR.Interaction.Toolkit;

    [AddComponentMenu("Haven XR/Socket/HXR Socket Target")]
    [RequireComponent(typeof(XRBaseInteractable))]
    public class HavenSocketTarget : MonoBehaviour
    {
        private static readonly Dictionary<int, string> SocketTargetMap = new Dictionary<int, string>();

#pragma warning disable 0649
        [SerializeField] private XRBaseInteractable interactable;
        [SerializeField] private string socketTargetName;
        [SerializeField] private bool disableInteractableOnSocketed;
        [SerializeField] private SelectEnterEvent onSocketedEvent;
#pragma warning restore 0649

        public static string GetSocketTargetName(XRBaseInteractable interactable)
        {
            if (SocketTargetMap.TryGetValue(interactable.GetInstanceID(), out string socketTargetName))
            {
                return socketTargetName;
            }

            return null;
        }

        private void Awake()
        {
            if (this.interactable && string.IsNullOrWhiteSpace(this.socketTargetName) == false)
            {
                this.interactable.selectEntered.AddListener(this.SelectedSwitch);
            }
            else
            {
                Debug.LogError($"HavenSocketTarget {this.name} has no interactable or socketTargetName and will not work.", this);
            }
        }

        private void OnDestroy()
        {
            if (this.interactable && string.IsNullOrWhiteSpace(this.socketTargetName) == false)
            {
                this.interactable.selectEntered.RemoveListener(this.SelectedSwitch);
            }
        }

        private void OnEnable()
        {
            if (this.interactable && string.IsNullOrWhiteSpace(this.socketTargetName) == false)
            {
                SocketTargetMap.Add(this.interactable.GetInstanceID(), this.socketTargetName);
            }
        }

        private void OnDisable()
        {
            if (this.interactable && string.IsNullOrWhiteSpace(this.socketTargetName) == false)
            {
                SocketTargetMap.Remove(this.interactable.GetInstanceID());
            }
        }

        private void SelectedSwitch(SelectEnterEventArgs selectEnterEventArgs)
        {
            var socketInteractor = selectEnterEventArgs.interactorObject as HavenSocket;

            if (socketInteractor == null || socketInteractor.SocketTargetName != this.socketTargetName)
            {
                return;
            }

            if (this.disableInteractableOnSocketed)
            {
                this.interactable.enabled = false;
            }

            try
            {
                this.onSocketedEvent?.Invoke(selectEnterEventArgs);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}

#endif
