//-----------------------------------------------------------------------
// <copyright file="HavenInteractableUtil.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost.Haven
{
    using UnityEngine;
    using UnityEngine.XR.Interaction.Toolkit;

    public static class HavenInteractableUtil
    {
        public static void Setup(XRBaseInteractable interactable, HavenLayer layer)
        {
            // Always make sure the we're not saving a reference to the interaction manager
            if (interactable.interactionManager != null)
            {
                interactable.interactionManager = null;
                EditorUtil.SetDirty(interactable);
            }

            // Auto populating a collider if it already exists
            if (interactable.colliders.Count == 0)
            {
                var colliders = interactable.GetComponentsInChildren<Collider>();

                if (colliders.Length > 0)
                {
                    foreach (var collider in colliders)
                    {
                        interactable.colliders.Add(collider);
                    }

                    EditorUtil.SetDirty(interactable);
                }
            }

            // Setting collider layers
            HavenLayerUtil.SetLayerOnColliders(interactable, layer);
        }

        [EditorEvents.OnEnterPlayMode]
        public static void SetupPhysicsLayerCollisions()
        {
            string interactableLayerName = HavenLayerUtil.GetLayerName(HavenLayer.Interactable);
            string interactorLayerName = HavenLayerUtil.GetLayerName(HavenLayer.Interactor);
            string teleportLayerName = HavenLayerUtil.GetLayerName(HavenLayer.Teleport);

            var interactableLayer = LayerMask.NameToLayer(interactableLayerName);
            var interactorLayer = LayerMask.NameToLayer(interactorLayerName);
            var teleportLayer = LayerMask.NameToLayer(teleportLayerName);

            if (interactableLayer == -1 || interactorLayer == -1 || teleportLayer == -1)
            {
                Debug.LogError($"Physics Layers are missing layer(s) ({interactableLayerName}, {interactorLayerName} and/or {teleportLayerName}), please add these layers if you want Have Interactions to work.");
                return;
            }

            if (Physics.GetIgnoreLayerCollision(interactableLayer, interactorLayer))
            {
                Physics.IgnoreLayerCollision(interactableLayer, interactorLayer, false);
            }
        }
    }
}

#endif
