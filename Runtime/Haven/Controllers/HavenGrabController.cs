//-----------------------------------------------------------------------
// <copyright file="GrabXRController.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost.XR
{
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.XR.Interaction.Toolkit;

    public sealed class HavenGrabController : XRController
    {
        #pragma warning disable 0649
        [SerializeField] private XRBaseControllerInteractor interactor;
        #pragma warning restore 0649

        private bool isPressed = false;
        private bool previousIsPressed = false;

        private int lastCalculatedFrame;
        private bool isActive;
        private bool wasActivatedThisFrame;
        private bool wasDeactivatedThisFrame;

        public bool HasSelection
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.interactor.hasSelection;
        }

        public bool HasHover
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.interactor.hasHover;
        }

        protected override void Awake()
        {
            base.Awake();

            this.updateTrackingType = UpdateType.BeforeRender;
            this.enableInputActions = false;
            this.enableInputTracking = false;
        }

        public void EnableGrab()
        {
            this.interactor.enabled = true;
            this.enabled = true;
        }

        public void DisableGrab()
        {
            this.interactor.enabled = false;
            this.enabled = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartGrab()
        {
            this.isPressed = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EndGrab()
        {
            this.isPressed = false;
        }

        protected override void ApplyControllerState(XRInteractionUpdateOrder.UpdatePhase updatePhase, XRControllerState controllerState)
        {
            if (controllerState == null)
            {
                return;
            }

            int currentFrame = Time.frameCount;

            if (this.lastCalculatedFrame != currentFrame)
            {
                this.lastCalculatedFrame = currentFrame;

                this.isActive = this.isPressed;
                this.wasActivatedThisFrame = this.previousIsPressed == false && this.isPressed;
                this.wasDeactivatedThisFrame = this.previousIsPressed && this.isPressed == false;
                this.previousIsPressed = this.isPressed;
            }

            controllerState.selectInteractionState.active = this.isActive;
            controllerState.selectInteractionState.activatedThisFrame = this.wasActivatedThisFrame;
            controllerState.selectInteractionState.deactivatedThisFrame = this.wasDeactivatedThisFrame;

            base.ApplyControllerState(updatePhase, controllerState);
        }
    }
}
