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
    using UnityEngine.Events;
    using UnityEngine.XR.Interaction.Toolkit;

    [AddComponentMenu("Haven XR/Socket/HXR Socket Target")]
    [RequireComponent(typeof(XRBaseInteractable))]
    public class HavenSocketTarget : MonoBehaviour, IAwake, IValidate
    {
        private static readonly Dictionary<int, string> SocketTargetMap = new Dictionary<int, string>();

#pragma warning disable 0649
        [SerializeField] private Rigidbody interactableRigidbody;
        [SerializeField] private XRBaseInteractable interactable;
        [SerializeField] private string socketTargetName;
        [SerializeField] private UnityEvent<XRBaseInteractor> onSocketed;
#pragma warning restore 0649

        public static string GetSocketTargetName(XRBaseInteractable interactable)
        {
            if (SocketTargetMap.TryGetValue(interactable.GetInstanceID(), out string socketTargetName))
            {
                return socketTargetName;
            }

            return null;
        }

        public void Validate(List<ValidationError> errors)
        {
            this.AssertNotNull(errors, this.interactable, nameof(this.interactable));
            this.AssertValidString(errors, this.socketTargetName, nameof(this.socketTargetName));
        }

        public void OnAwake()
        {
            this.interactable.selectEntered.AddListener(this.SelectEntered);
        }

        public void DisableInteractable()
        {
            this.enabled = false;
            this.interactable.enabled = false;

            if (this.interactableRigidbody != null)
            {
                this.interactableRigidbody.useGravity = false;
                this.interactableRigidbody.isKinematic = true;
            }
        }

        private void Awake() => ActivationManager.Register(this);

        private void OnDestroy()
        {
            if (this.interactable && string.IsNullOrWhiteSpace(this.socketTargetName) == false)
            {
                this.interactable.selectEntered.RemoveListener(this.SelectEntered);
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

        private void OnValidate()
        {
            EditorUtil.SetIfNull(this, ref this.interactable);
            EditorUtil.SetIfNull(this, ref this.interactableRigidbody);
        }

        private void SelectEntered(SelectEnterEventArgs selectEnterEventArgs)
        {
            var socketInteractor = selectEnterEventArgs.interactorObject as HavenSocket;

            if (socketInteractor == null || socketInteractor.SocketTargetName != this.socketTargetName)
            {
                return;
            }

            try
            {
                this.onSocketed?.Invoke(selectEnterEventArgs.interactorObject as XRBaseInteractor);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
            }
        }
    }
}

#endif
