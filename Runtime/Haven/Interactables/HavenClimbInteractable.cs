//-----------------------------------------------------------------------
// <copyright file="HavenClimbInteractable.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost.XR
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.XR.Interaction.Toolkit;

    public class HavenClimbInteractable : XRBaseInteractable, IAwake
    {
        private static readonly Dictionary<int, HavenHand> Hands = new Dictionary<int, HavenHand>();

#pragma warning disable 0649
        [SerializeField] private Collider climbCollider;
        [SerializeField] private Rigidbody climbRigidbody;
#pragma warning restore 0649

        public void OnAwake()
        {
            Debug.Assert(this.climbCollider != null, this);
            Debug.Assert(this.climbRigidbody != null, this);
            Debug.Assert(this.climbRigidbody.isKinematic, this);
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

            if (this.climbCollider == null)
            {
                this.climbCollider = this.GetComponent<Collider>();
                EditorUtil.SetDirty(this);
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

            HavenInteractableUtil.Setup(this, HavenLayer.Interactable);
        }
    }
}
