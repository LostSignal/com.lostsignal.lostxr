//-----------------------------------------------------------------------
// <copyright file="CharacterController.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost.Haven
{
    using System.Runtime.CompilerServices;
    using Lost.XR;
    using UnityEngine;

    public class HavenHand : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private Hand hand;
        [SerializeField] private HavenRig havenRig;
        [SerializeField] private HavenGrabController directGrab;
        [SerializeField] private HavenGrabController rayGrab;
        [SerializeField] private HavenTeleportController teleport;
        [SerializeField] private HavenUIController ui;
        [SerializeField] private Transform handTransform;
        [SerializeField] private Vector3 postitionOffset;
        [SerializeField] private Quaternion rotationOffset;
        [SerializeField] private Mode debugMode;
#pragma warning restore 0649

        private float previousTrigger;
        private float previousGrip;
        private Mode currentMode;
        private bool disableTeleport;
        private float teleportReleaseTime;

        public enum Mode
        {
            None,
            DirectGrab,
            RayGarb,
            UI,
            Teleport,
        }

        public HavenRig Rig
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.havenRig;
        }

        public Hand Hand
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.hand;
        }

        public bool IsTeleporting
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public bool DidJustTeleport
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Time.realtimeSinceStartup - this.teleportReleaseTime < 0.25f;
        }

        public bool DisableTeleport
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.disableTeleport;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.disableTeleport = value;
        }

        private void Awake()
        {
            this.debugMode = Mode.DirectGrab;
            this.SetMode(Mode.DirectGrab);
        }

        private void SetMode(Mode mode)
        {
            if (mode != this.currentMode)
            {
                // if (mode == Mode.DirectGrab)
                // {
                //     this.directGrab.EnableGrab();
                // }
                // else
                // {
                //     this.directGrab.DisableGrab();
                // }
                // 
                // if (mode == Mode.RayGarb)
                // {
                //     this.rayGrab.EnableGrab();
                // }
                // else
                // {
                //     this.rayGrab.DisableGrab();
                // }
                // 
                // if (mode == Mode.Teleport)
                // {
                //     this.teleport.EnableTeleport();
                // }
                // else
                // {
                //     this.teleport.DisableTeleport();
                // }
                // 
                // if (mode == Mode.UI)
                // {
                //     this.ui.EnableUISelect();
                // }
                // else
                // {
                //     this.ui.DisableUISelect();
                // }
            }
        }

        private void Update()
        {
            var controllersState = XRControllersState.Instance;
            controllersState.UpdateValues();

            if (this.hand == Hand.Left ? controllersState.LeftSecondaryButton.WasPressedThisFrame : controllersState.RightSecondaryButton.WasPressedThisFrame)
            {
                this.debugMode = (Mode)(((int)this.debugMode + 1) % 5);
            }

            this.SetMode(this.debugMode);

            if (this.hand == Hand.Right)
            {
                this.UpdateHand(
                    controllersState.RightPosition,
                    controllersState.RightRotation,
                    controllersState.RightGrip,
                    controllersState.RightTrigger,
                    controllersState.RightStick);
            }
            else
            {
                this.UpdateHand(
                    controllersState.LeftPosition,
                    controllersState.LeftRotation,
                    controllersState.LeftGrip,
                    controllersState.LeftTrigger,
                    controllersState.LeftStick);
            }
        }

        private void UpdateHand(Vector3 position, Quaternion rotation, float grip, float trigger, Vector2 stick)
        {
            this.handTransform.localPosition = position + this.postitionOffset;
            this.handTransform.localRotation = rotation * this.rotationOffset;

            // Calculating Grabbing
            float currentGrip = grip;
            float gripDelta = currentGrip - this.previousGrip;

            if (gripDelta > 0.01 || currentGrip > 0.9f)
            {
                if (this.directGrab.HasHover || this.directGrab.HasSelection)
                {
                    // TODO [bgish]: Disable rayGrab and UI
                    this.directGrab.StartGrab();
                }
                else if (this.rayGrab.HasHover || this.rayGrab.HasSelection)
                {
                    // TODO [bgish]: Disable directGrab and UI
                    this.rayGrab.StartGrab();
                }
            }
            else if (gripDelta < -0.01 || currentGrip < 0.1f)
            {
                this.directGrab.EndGrab();
                this.rayGrab.EndGrab();
            }

            // Calculating Activation
            if (this.directGrab.HasSelection)
            {
                if (trigger > 0.05f)
                {
                    this.directGrab.StartActivate();
                }
                else
                {
                    this.directGrab.EndActivate();
                }
            }

            if (this.rayGrab.HasSelection)
            {
                if (trigger > 0.05f)
                {
                    this.rayGrab.StartActivate();
                }
                else
                {
                    this.rayGrab.EndActivate();
                }
            }

            this.previousGrip = currentGrip;

            // Calculating Teleporting
            if (this.DisableTeleport == false)
            {
                if (stick.y > 0.8f)
                {
                    this.IsTeleporting = true;
                }
                else if (stick.sqrMagnitude < 0.25f)
                {
                    if (this.IsTeleporting)
                    {
                        this.teleportReleaseTime = Time.realtimeSinceStartup;
                    }

                    this.IsTeleporting = false;
                }

                if (this.IsTeleporting)
                {
                    this.teleport.EnableTeleport();
                }
                else
                {
                    this.teleport.DisableTeleport();
                }
            }
            else
            {
                this.IsTeleporting = false;
                this.teleport.DisableTeleport();
            }

            // Calculating UI
            float currentTrigger = trigger;
            float triggerDelta = currentTrigger - this.previousTrigger;

            if (triggerDelta > 0.01 || triggerDelta > 0.9f)
            {
                this.ui.StartUIClick();
            }
            else if (triggerDelta < -0.01 || triggerDelta < 0.1f)
            {
                this.ui.EndUIClick();
            }

            this.previousTrigger = currentTrigger;
        }
    }
}

#endif