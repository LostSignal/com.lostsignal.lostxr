//-----------------------------------------------------------------------
// <copyright file="HavenSocket.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost.Haven
{
    using UnityEngine;
    using UnityEngine.XR.Interaction.Toolkit;

    [AddComponentMenu("Haven XR/Socket/HXR Socket")]
    public class HavenSocket : XRSocketInteractor, IAwake
    {
#pragma warning disable 0649
        [SerializeField] private HavenSocketSettingsObject havenSocketSettings;
        [SerializeField] private bool onlyAllowSpecificSocketTarget;

        [ShowIf("onlyAllowSpecificSocketTarget", true)]
        [SerializeField] private string socketTargetName;
#pragma warning restore 0649

        public string SocketTargetName => this.socketTargetName;

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


        public void OnAwake()
        {
            this.AssertNotNull(this.havenSocketSettings, nameof(this.havenSocketSettings));

            this.havenSocketSettings.Apply(this);
        }

        protected override void Awake()
        {
            base.Awake();
            ActivationManager.Register(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (this.havenSocketSettings == null)
            {
                this.havenSocketSettings = EditorUtil.GetAssetByGuid<HavenSocketSettingsObject>("c336bbd69f11b7d48aef5ba5aea19c37");
                EditorUtil.SetDirty(this);
            }
        }
#endif
    }
}

#endif
