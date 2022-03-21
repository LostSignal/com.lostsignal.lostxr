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
    using UnityEngine.XR.Interaction.Toolkit;

    [AddComponentMenu("Haven XR/Interactables/HXR Teleport")]
    public class HavenTeleport : BaseTeleportationInteractable, IAwake
    {
#pragma warning disable 0649
        [SerializeField] private TeleportType type;
        [SerializeField] private bool setScaleOnTeleport;
        [SerializeField] private float rigScale = 1.0f;

        [Tooltip("The Transform that represents the teleportation destination.")]
        [SerializeField] private Transform anchorOverrideTransform;
#pragma warning restore 0649

        private System.Action<IXRInteractor, TeleportRequest> onTeleport;

        public event System.Action<IXRInteractor, TeleportRequest> OnTeleport
        {
            add => this.onTeleport += value;
            remove => this.onTeleport -= value;
        }

        public enum TeleportType
        {
            Area,
            Anchor,
        }

        private Transform AnchorOverrideTransform => this.anchorOverrideTransform != null ? this.anchorOverrideTransform : this.transform;

        public void OnAwake()
        {
            this.teleportationProvider = HavenRig.Instance.TeleportationProvider;
            this.interactionManager = XRInteractionHelper.XRInteractionManagerInstance;
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
                };
            }
            else if (this.type == TeleportType.Anchor)
            {
                var anchorOverrideTransform = this.AnchorOverrideTransform;

                teleportRequest = new TeleportRequest
                {
                    destinationPosition = anchorOverrideTransform.position,
                    destinationRotation = anchorOverrideTransform.rotation,
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

            if (this.teleportTrigger != TeleportTrigger.OnSelectExited)
            {
                this.teleportTrigger = TeleportTrigger.OnSelectExited;
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
    }
}

#endif
