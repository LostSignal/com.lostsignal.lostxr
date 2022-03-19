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
        [SerializeField] private HavenRigSettings settings = HavenRigSettings.Default;
        [SerializeField] private HavenSnapTurnProvider snapTurnProvider;
        [SerializeField] private HavenContinuousTurnProvider continuousTurnProvider;
        [SerializeField] private HavenContinuousMoveProvider continuousMoveProvider;
        [SerializeField] private CharacterControllerDriver characterControllerDriver;
        [SerializeField] private TeleportationProvider teleportationProvider;

        [SerializeField] private HavenHand leftHand;
        [SerializeField] private HavenHand rightHand;
        [SerializeField] private float movementSpeed;

        [SerializeField] private Transform headTransform;
        [SerializeField] private Transform leftHandTransform;
        [SerializeField] private Transform rightHandTransform;
        #pragma warning restore 0649

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

        public void StartClimbing(Hand hand)
        {
            //// TODO [bgish]: Implement
            //// Debug.Log($"StartClimbing({hand})");
        }

        public void StopClimbing(Hand hand)
        {
            //// TODO [bgish]: Implement
            //// Debug.Log($"StopClimbing({hand})");
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
