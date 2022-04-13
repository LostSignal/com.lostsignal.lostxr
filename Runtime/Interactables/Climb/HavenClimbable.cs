//-----------------------------------------------------------------------
// <copyright file="HavenClimbable.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost.Haven
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.XR.Interaction.Toolkit;

    [AddComponentMenu("Haven XR/Interactables/HXR Climbable")]
    public class HavenClimbable : XRBaseInteractable, IAwake
    {
        private static readonly Dictionary<int, HavenHand> Hands = new Dictionary<int, HavenHand>();

#pragma warning disable 0649
        [SerializeField] private HavenClimbableSettingsObject havenClimbableSettings;
        [SerializeField] private Rigidbody climbRigidbody;
#pragma warning restore 0649

        public void OnAwake()
        {
            this.AssertNotNull(this.havenClimbableSettings, nameof(this.havenClimbableSettings));
            this.AssertNotNull(this.climbRigidbody, nameof(this.climbRigidbody));
            this.AssertTrue(this.climbRigidbody.isKinematic, nameof(this.climbRigidbody.isKinematic));

            this.havenClimbableSettings.Apply(this);
        }

        protected override void Awake()
        {
            base.Awake();
            ActivationManager.Register(this);
        }

        public override bool IsHoverableBy(IXRHoverInteractor interactor)
        {
            return interactor is XRDirectInteractor && base.IsHoverableBy(interactor);
        }

        public override bool IsSelectableBy(IXRSelectInteractor interactor)
        {
            return interactor is XRDirectInteractor && base.IsSelectableBy(interactor);
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            var havenHand = this.GetHavenHand(args.interactorObject as XRBaseInteractor);

            if (havenHand != null)
            {
                havenHand.Rig.StartClimbing(havenHand.Hand);
            }

            base.OnSelectEntered(args);
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            var havenHand = this.GetHavenHand(args.interactorObject as XRBaseInteractor);

            if (havenHand != null)
            {
                havenHand.Rig.StopClimbing(havenHand.Hand);
            }

            base.OnSelectExited(args);
        }

        private HavenHand GetHavenHand(XRBaseInteractor interactor)
        {
            if (interactor == null)
            {
                return null;
            }

            if (Hands.TryGetValue(interactor.GetInstanceID(), out HavenHand havenHand) == false)
            {
                havenHand = interactor.transform.parent.GetComponent<HavenHand>();

                if (havenHand != null)
                {
                    Hands.Add(interactor.GetInstanceID(), havenHand);
                }
            }

            return havenHand;
        }

        [EditorEvents.OnExitPlayMode]
        private static void ResetHands()
        {
            Hands.Clear();
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (this.climbRigidbody == null)
            {
                this.climbRigidbody = this.GetComponent<Rigidbody>();
                EditorUtil.SetDirty(this);
            }

            if (this.climbRigidbody != null && this.climbRigidbody.isKinematic == false)
            {
                this.climbRigidbody.isKinematic = true;
                EditorUtil.SetDirty(this);
            }

            if (this.selectMode != InteractableSelectMode.Single)
            {
                this.selectMode = InteractableSelectMode.Single;
                EditorUtil.SetDirty(this);
            }

            if (this.havenClimbableSettings == null)
            {
                this.havenClimbableSettings = EditorUtil.GetAssetByGuid<HavenClimbableSettingsObject>("c533b0e320be29a468a40f3bad7648b2");
                EditorUtil.SetDirty(this);
            }

            HavenInteractableUtil.Setup(this, HavenLayer.Interactable);
        }
    }
}

#endif
