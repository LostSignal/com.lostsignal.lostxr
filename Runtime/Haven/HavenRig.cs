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
        private static HavenRig instance;

        #pragma warning disable 0649
        [Header("Settings")]
        [SerializeField] private HavenRigSettings settings = HavenRigSettings.Default;
        [SerializeField] private float movementSpeed;

        [Header("Providers")]
        [SerializeField] private HavenSnapTurnProvider snapTurnProvider;
        [SerializeField] private HavenContinuousTurnProvider continuousTurnProvider;
        [SerializeField] private HavenContinuousMoveProvider continuousMoveProvider;
        [SerializeField] private CharacterControllerDriver characterControllerDriver;
        [SerializeField] private TeleportationProvider teleportationProvider;

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

        public HavenRigSettings Settings
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.settings;
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
            if (this.isClimbingWithLeftHand || this.isClimbingWithRightHand)
            {
                Vector3 offset = Vector3.zero;
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

                offset /= activeHands;

                this.transform.localPosition += offset;
            }
        }

        public void StartClimbing(Hand hand)
        {
            if (hand == Hand.Left)
            {
                this.isClimbingWithLeftHand = true;
                this.leftHandPreviousPosition = this.leftHand.transform.position;
            }
            else if (hand == Hand.Right)
            {
                this.isClimbingWithRightHand = true;
                this.rightHandPreviousPosition = this.rightHand.transform.position;
            }
            else
            {
                throw new NotImplementedException();
            }
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
        }

        [ExposeInEditor("Settings Changed")]
        private void OnSettingsChanged()
        {
            var settings = this.GetSettings();

            // Updating settings for all providers
            this.snapTurnProvider.UpdateSettings(settings, this.leftHand, this.rightHand);
            this.continuousTurnProvider.UpdateSettings(settings, this.leftHand, this.rightHand);
            this.continuousMoveProvider.UpdateSettings(
                settings,
                this.movementSpeed,
                this.headTransform,
                this.leftHand,
                this.rightHand);

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

        private HavenRigSettings GetSettings()
        {
            // TODO [bgish]: Read in settings from DataStore
            return this.settings;
        }
    }
}
