//-----------------------------------------------------------------------
// <copyright file="HavenRig.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost.XR
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.XR.Interaction.Toolkit;

    public class HavenRig : MonoBehaviour
    {
        private static readonly Vector3 Zero = new Vector3(0.0f, 0.0f, 0.0f);

        private static HavenRig instance;

        #pragma warning disable 0649
        [Header("Settings")]
        [SerializeField] private FloatSetting height;
        [SerializeField] private IntSetting sitStand;
        [SerializeField] private IntSetting movementMode;
        [SerializeField] private IntSetting comfortMode;

        [SerializeField] private BoolSetting enableStrafe;
        [SerializeField] private IntSetting movementHand;
        [SerializeField] private IntSetting movementSource;

        [SerializeField] private IntSetting turnMode;
        [SerializeField] private BoolSetting allowTurnAround;
        [SerializeField] private FloatSetting snapTurnDegrees;
        [SerializeField] private FloatSetting continuousTurnSpeed;
        [SerializeField] private float movementSpeed;

        [Header("Providers")]
        [SerializeField] private HavenSnapTurnProvider snapTurnProvider;
        [SerializeField] private HavenContinuousTurnProvider continuousTurnProvider;
        [SerializeField] private HavenContinuousMoveProvider continuousMoveProvider;
        [SerializeField] private CharacterControllerDriver characterControllerDriver;
        [SerializeField] private TeleportationProvider teleportationProvider;
        [SerializeField] private CharacterController characterController;

        [Header("Hands")]
        [SerializeField] private HavenHand leftHand;
        [SerializeField] private HavenHand rightHand;
        
        [Header("Transforms")]
        [SerializeField] private Transform headTransform;
        [SerializeField] private Transform leftHandTransform;
        [SerializeField] private Transform rightHandTransform;
        #pragma warning restore 0649

        private bool isClimbingWithLeftHand;
        private bool isClimbingWithRightHand;

        private Vector3 leftHandPreviousPosition;
        private Vector3 rightHandPreviousPosition;

        public static HavenRig Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => instance;
        }

        public float RigScale
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 1.0f;
        }

        public TeleportationProvider TeleportationProvider
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.teleportationProvider;
        }

        public Transform HeadTransform
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.headTransform;
        }

        public Transform LeftHandTransform
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.leftHandTransform;
        }

        public Transform RightHandTransform
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.rightHandTransform;
        }

        public static Coroutine WaitForRig()
        {
            Debug.LogError("WaitForRig Called!");
            return CoroutineRunner.Instance.StartCoroutine(Coroutine());

            IEnumerator Coroutine()
            {
                while (instance == null)
                {
                    yield return null;
                }
            }
        }

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Multiple Rigs Detected!", this);
                return;
            }

            instance = this;
            this.OnSettingsChanged();
        }

        private void Update()
        {
            this.UpdateClimbing();
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                this.OnSettingsChanged();
            }
        }
        #endif

        private void UpdateClimbing()
        {
            if (this.isClimbingWithLeftHand || this.isClimbingWithRightHand)
            {
                Vector3 offset = Zero;
                int activeHands = 0;

                if (this.isClimbingWithLeftHand)
                {
                    activeHands++;
                    Vector3 leftHandPosition = this.leftHandTransform.position;
                    offset += this.leftHandPreviousPosition - leftHandPosition;
                    this.leftHandPreviousPosition = leftHandPosition;
                }

                if (this.isClimbingWithRightHand)
                {
                    activeHands++;
                    Vector3 rightHandPosition = this.rightHandTransform.position;
                    offset += this.rightHandPreviousPosition - rightHandPosition;
                    this.rightHandPreviousPosition = rightHandPosition;
                }

                // NOTE [bgish]: Not sure if averaging is the best route if you have multiple hands climbing at once, test and find out
                offset /= activeHands;

                this.characterController.Move(offset);
                this.leftHandPreviousPosition += offset;
                this.rightHandPreviousPosition += offset;
            }
        }

        public void StartClimbing(Hand hand)
        {
            if (hand == Hand.Left)
            {
                this.isClimbingWithLeftHand = true;
                this.leftHandPreviousPosition = this.leftHandTransform.position;
            }
            else if (hand == Hand.Right)
            {
                this.isClimbingWithRightHand = true;
                this.rightHandPreviousPosition = this.rightHandTransform.position;
            }
            else
            {
                throw new NotImplementedException();
            }

            // Disable all other movement when in climbing mode
            this.continuousMoveProvider.enabled = false;
            this.continuousTurnProvider.enabled = false;
            this.snapTurnProvider.enabled = false;
        }

        public void StopClimbing(Hand hand)
        {
            if (hand == Hand.Left)
            {
                this.isClimbingWithLeftHand = false;
            }
            else if (hand == Hand.Right)
            {
                this.isClimbingWithRightHand = false;
            }
            else
            {
                throw new NotImplementedException();
            }

            if (this.isClimbingWithLeftHand == false && this.isClimbingWithRightHand == false)
            {
                this.UpdateProviderSettings(this.GetSettings());
            }
        }

        [ExposeInEditor("Settings Changed")]
        private void OnSettingsChanged()
        {
            var settings = this.GetSettings();

            // Updating settings for all providers
            this.UpdateProviderSettings(settings);

            // Enabling / Disabling Teleport and Turn Around
            if (settings.MovementMode == MovementMode.ContinuousOnly)
            {
                this.leftHand.DisableTeleport = true;
                this.rightHand.DisableTeleport = true;
            }
            else if (settings.MovementMode == MovementMode.ContinuousAndTeleport)
            {
                this.leftHand.DisableTeleport = settings.MovementHand == Hand.Left;
                this.rightHand.DisableTeleport = settings.MovementHand == Hand.Right;
            }
            else if (settings.MovementMode == MovementMode.TeleportOnly)
            {
                this.leftHand.DisableTeleport = false;
                this.rightHand.DisableTeleport = false;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void UpdateProviderSettings(HavenRigSettings settings)
        {
            this.snapTurnProvider.UpdateSettings(settings, this.leftHand, this.rightHand);

            this.continuousTurnProvider.UpdateSettings(settings, this.leftHand, this.rightHand);

            this.continuousMoveProvider.UpdateSettings(
                settings,
                this.movementSpeed,
                this.headTransform,
                this.leftHand,
                this.rightHand);
        }

        private HavenRigSettings GetSettings()
        {
            //// [SerializeField] private FloatSetting height;
            //// [SerializeField] private IntSetting sitStand;
            //// [SerializeField] private IntSetting movementMode;
            //// [SerializeField] private IntSetting comfortMode;
            //// 
            //// [SerializeField] private BoolSetting enableStrafe;
            //// [SerializeField] private IntSetting movementHand;
            //// [SerializeField] private IntSetting movementSource;
            //// 
            //// [SerializeField] private IntSetting turnMode;
            //// [SerializeField] private BoolSetting allowTurnAround;
            //// [SerializeField] private FloatSetting snapTurnDegrees;
            //// [SerializeField] private FloatSetting continuousTurnSpeed;

            return new HavenRigSettings
            {
                // General
                MovementMode = this.movementMode.Value == 0 ? MovementMode.ContinuousAndTeleport :
                               this.movementMode.Value == 1 ? MovementMode.ContinuousOnly : MovementMode.TeleportOnly,
                
                // Movement
                AllowStrafe = this.enableStrafe.Value,
                MovementHand = this.movementHand.Value == 0 ? Hand.Left : Hand.Right,
                MovementSource = this.movementSource.Value == 0 ? ContinuousMovementSource.Head :
                                 this.movementSource.Value == 1 ? ContinuousMovementSource.LeftHand : ContinuousMovementSource.RightHand,

                // Turning
                TurnMode = this.turnMode.Value == 0 ? TurnMode.Continuous : TurnMode.Snap,
                ContinuousTurnSpeed = this.continuousTurnSpeed.Value,
                SnapTurnAmmount = this.snapTurnDegrees.Value,
                EnableTurnAround = this.allowTurnAround.Value,
            };
        }
    }
}
