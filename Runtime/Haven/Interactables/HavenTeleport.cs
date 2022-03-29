//-----------------------------------------------------------------------
// <copyright file="HavenTeleport.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if true // UNITY

namespace Lost.XR
{
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.XR.Interaction.Toolkit;

    [AddComponentMenu("Haven XR/Interactables/HXR Teleport")]
    public class HavenTeleport : BaseTeleportationInteractable, IAwake
    {
#pragma warning disable 0649
        [SerializeField] private TeleportType type;

        [ShowIf("type", TeleportType.Anchor)]
        [Tooltip("The Transform that represents the teleportation destination.")]
        [SerializeField] private Transform anchorTransform;

        [ShowIf("type", TeleportType.Anchor)]
        [SerializeField] private bool matchAnchorOrientation = true;

        [Header("Scaling")]
        [SerializeField] private bool setScaleOnTeleport;
        [SerializeField] private float rigScale = 1.0f;

        [Header("Hover")]
        [SerializeField] private UnityEvent onHoverStart;
        [SerializeField] private UnityEvent onHoverStop;
#pragma warning restore 0649

        private System.Action<IXRInteractor, TeleportRequest> onTeleport;

        public event System.Action<IXRInteractor, TeleportRequest> OnTeleport
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            add => this.onTeleport += value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            remove => this.onTeleport -= value;
        }

        public enum TeleportType
        {
            Area,
            Anchor,
        }

        private Transform AnchorOverrideTransform => this.anchorTransform != null ? this.anchorTransform : this.transform;

        public void OnAwake()
        {
            this.teleportationProvider = HavenRig.Instance.TeleportationProvider;
            this.interactionManager = XRInteractionHelper.XRInteractionManagerInstance;

            this.firstHoverEntered.AddListener(this.OnFirstHoverEnter);
            this.lastHoverExited.AddListener(this.OnLastHoverExit);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            this.firstHoverEntered.RemoveListener(this.OnFirstHoverEnter);
            this.lastHoverExited.RemoveListener(this.OnLastHoverExit);
        }

        protected void OnDrawGizmos()
        {
            if (this.type == TeleportType.Anchor)
            {
                Gizmos.color = Color.blue;
                GizmoHelpers.DrawWireCubeOriented(this.AnchorOverrideTransform.position, this.AnchorOverrideTransform.rotation, 1f);
                GizmoHelpers.DrawAxisArrows(this.AnchorOverrideTransform, 1f);
            }
        }

        protected override bool GenerateTeleportRequest(IXRInteractor interactor, RaycastHit raycastHit, ref TeleportRequest teleportRequest)
        {
            if (this.type == TeleportType.Area)
            {
                teleportRequest = new TeleportRequest
                {
                    destinationPosition = raycastHit.point,
                    destinationRotation = this.transform.rotation,
                    matchOrientation = MatchOrientation.WorldSpaceUp,
                };
            }
            else if (this.type == TeleportType.Anchor)
            {
                var anchorOverrideTransform = this.AnchorOverrideTransform;

                teleportRequest = new TeleportRequest
                {
                    destinationPosition = anchorOverrideTransform.position,
                    destinationRotation = anchorOverrideTransform.rotation,
                    matchOrientation = this.matchAnchorOrientation ? MatchOrientation.TargetUpAndForward : MatchOrientation.WorldSpaceUp,
                };
            }
            else
            {
                throw new System.NotImplementedException();
            }

            if (this.setScaleOnTeleport)
            {
                //// CoroutineRunner.Instance.ExecuteDelayed(0.1f, () => HavenRig.GetRig().SetScale(this.rigScale));
            }

            this.onTeleport?.Invoke(interactor, teleportRequest);

            return true;
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }

            HavenInteractableUtil.Setup(this, HavenLayer.Teleport);

            if (this.teleportTrigger != TeleportTrigger.OnSelectEntered)
            {
                this.teleportTrigger = TeleportTrigger.OnSelectEntered;
                EditorUtil.SetDirty(this);
            }

            if (this.type == TeleportType.Anchor && this.anchorTransform == null)
            {
                this.anchorTransform = this.transform;
                EditorUtil.SetDirty(this);
            }

            #pragma warning disable CS0618
            if (this.interactionLayerMask != 0)
            {
                this.interactionLayerMask = 0;
                EditorUtil.SetDirty(this);
            }
            #pragma warning restore

            if (this.interactionLayers != 0)
            {
                this.interactionLayers = 0;
                EditorUtil.SetDirty(this);
            }

            if (this.teleportationProvider != null)
            {
                this.teleportationProvider = null;
                EditorUtil.SetDirty(this);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            ActivationManager.Register(this);
        }

        private void OnFirstHoverEnter(HoverEnterEventArgs args)
        {
            this.onHoverStart.SafeInvoke();
        }

        private void OnLastHoverExit(HoverExitEventArgs args)
        {
            this.onHoverStop.SafeInvoke();
        }
    }
}

#endif
