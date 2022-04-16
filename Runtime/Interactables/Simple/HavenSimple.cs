//-----------------------------------------------------------------------
// <copyright file="HavenSimple.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost.Haven
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.XR.Interaction.Toolkit;

    [AddComponentMenu("Haven XR/Interactables/HXR Simple")]
    public class HavenSimple : XRSimpleInteractable, IAwake, IValidate
    {
#pragma warning disable 0649
        [SerializeField] private HavenSimpleSettingsObject havenSimpleSettings;
        [SerializeField] private bool disableRayGrab;
#pragma warning restore 0649

        public bool DisableRayGrab
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.disableRayGrab;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.disableRayGrab = value;
        }

        public void Validate(List<ValidationError> errors)
        {
            this.AssertNotNull(errors, this.havenSimpleSettings, nameof(this.havenSimpleSettings));
        }

        public void OnAwake()
        {
            this.havenSimpleSettings.Apply(this);
        }

        public override bool IsHoverableBy(IXRHoverInteractor interactor)
        {
            if (this.disableRayGrab && interactor is XRRayInteractor)
            {
                return false;
            }

            return base.IsHoverableBy(interactor);
        }

        public override bool IsSelectableBy(IXRSelectInteractor interactor)
        {
            if (this.disableRayGrab && interactor is XRRayInteractor)
            {
                return false;
            }

            return base.IsSelectableBy(interactor);
        }

        protected override void Awake()
        {
            base.Awake();
            ActivationManager.Register(this);
        }

        private void OnValidate()
        {
            EditorUtil.SetIfNull(this, ref this.havenSimpleSettings, "c533b0e320be29a468a40f3bad7648b2");

            HavenInteractableUtil.Setup(this, HavenLayer.Interactable);
        }
    }
}
