//-----------------------------------------------------------------------
// <copyright file="HavenSimpleInteractable.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.XR.Interaction.Toolkit;

    [AddComponentMenu("Haven XR/Interactables/HXR Simple")]
    public class HavenSimpleInteractable : XRSimpleInteractable
    {
#pragma warning disable 0649
        [SerializeField] private bool disableRayGrab;
#pragma warning restore 0649

        public bool DisableRayGrab
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.disableRayGrab;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.disableRayGrab = value;
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
    }
}
