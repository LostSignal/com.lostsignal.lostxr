//-----------------------------------------------------------------------
// <copyright file="HavenSocket.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost.Haven
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.XR.Interaction.Toolkit;

    [AddComponentMenu("Haven XR/Socket/HXR Socket")]
    public class HavenSocket : XRSocketInteractor, IAwake, IValidate
    {
#pragma warning disable 0649
        [SerializeField] private HavenSocketSettingsObject havenSocketSettings;
        [SerializeField] private bool disableInteractorAndInteractableOnSocketed;
        [SerializeField] private bool onlyAllowSpecificSocketTarget;
        [SerializeField] private UnityEvent<XRBaseInteractable> onSocketed;

        [ShowIf("onlyAllowSpecificSocketTarget", true)]
        [SerializeField] private string socketTargetName;
#pragma warning restore 0649

        public string SocketTargetName => this.socketTargetName;

        public void Validate(List<ValidationError> errors)
        {
            this.AssertNotNull(errors, this.havenSocketSettings, nameof(this.havenSocketSettings));
        }

        public void OnAwake()
        {
            this.havenSocketSettings.Apply(this);
        }

        public override bool CanHover(IXRHoverInteractable interactable)
        {
            bool canHover = base.CanHover(interactable);

            if (canHover && this.onlyAllowSpecificSocketTarget)
            {
                var xrBaseInteractable = interactable as XRBaseInteractable;

                if (xrBaseInteractable != null)
                {
                    canHover &= HavenSocketTarget.GetSocketTargetName(xrBaseInteractable) == this.socketTargetName;
                }
            }

            return canHover;
        }

        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            bool canSelect = base.CanSelect(interactable);

            if (canSelect && this.onlyAllowSpecificSocketTarget)
            {
                var xrBaseInteractable = interactable as XRBaseInteractable;

                if (xrBaseInteractable != null)
                {
                    canSelect &= HavenSocketTarget.GetSocketTargetName(xrBaseInteractable) == this.socketTargetName;
                }
            }

            return canSelect;
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            if (this.onSocketed != null)
            {
                this.onSocketed?.Invoke(args.interactableObject as XRBaseInteractable);
            }

            if (this.disableInteractorAndInteractableOnSocketed)
            {
                this.ExecuteDelayed(0.2f, () =>
                {
                    this.enabled = false;
                    this.socketActive = false;

                    var interactable = args.interactableObject as XRBaseInteractable;
                    var socketTarget = interactable != null ? interactable.GetComponent<HavenSocketTarget>() : null;

                    if (socketTarget != null)
                    {
                        var parent = this.attachTransform != null ? this.attachTransform : this.transform;
                        socketTarget.transform.SetParent(parent);
                        socketTarget.DisableInteractable();
                    }
                });
            }
        }

        protected override void Awake()
        {
            base.Awake();
            ActivationManager.Register(this);
        }

        private void OnValidate()
        {
            EditorUtil.SetIfNull(this, ref this.havenSocketSettings, "c336bbd69f11b7d48aef5ba5aea19c37");
        }
    }
}

#endif
