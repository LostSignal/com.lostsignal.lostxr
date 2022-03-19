//-----------------------------------------------------------------------
// <copyright file="HavenLayerUtil.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost.XR
{
    using System;
    using UnityEngine;
    using UnityEngine.XR.Interaction.Toolkit;

    public enum HavenLayer
    {
        Interactor,
        Interactable,
        Teleport,
    }

    public static class HavenLayerUtil
    {
        public static string GetLayerName(HavenLayer layer)
        {
            return layer == HavenLayer.Interactable ? "Interactable" :
                   layer == HavenLayer.Interactor ? "Interactor" :
                   layer == HavenLayer.Teleport ? "Teleport" :
                   throw new NotImplementedException();
        }

        public static void SetLayerOnInteractableAndAllColliders(XRBaseInteractable interactable, HavenLayer layer)
        {
            string layerName = GetLayerName(layer);
            int layerIndex = LayerMask.NameToLayer(layerName);

            if (layerIndex != -1)
            {
                // Making sure the main object is on the correct layer
                if (interactable.gameObject.layer != layerIndex)
                {
                    interactable.gameObject.layer = layerIndex;
                    EditorUtil.SetDirty(interactable.gameObject);
                }

                // Making sure all colliders are on the right layer
                foreach (var collider in interactable.colliders)
                {
                    if (collider.gameObject.layer != layerIndex)
                    {
                        collider.gameObject.layer = layerIndex;
                        EditorUtil.SetDirty(collider.gameObject);
                    }
                }
            }
            else 
            {
                Debug.LogError($"Error setting interactable to the \"{layerName}\" layer.  Please add layer to settings.", interactable);
            }
        }

        [EditorEvents.OnEnterPlayMode]
        private static void SetupPhysicsLayerCollisions()
        {
            var interactableLayer = LayerMask.NameToLayer("Interactable");
            var interactorLayer = LayerMask.NameToLayer("Interactor");
            var teleportLayer = LayerMask.NameToLayer("Teleport");

            if (interactableLayer == -1 || interactorLayer == -1 || teleportLayer == -1)
            {
                return;
            }

            if (Physics.GetIgnoreLayerCollision(interactableLayer, interactorLayer))
            {
                Physics.IgnoreLayerCollision(interactableLayer, interactorLayer, false);
            }
        }
    }
}
