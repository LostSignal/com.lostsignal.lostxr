//-----------------------------------------------------------------------
// <copyright file="HavenClimbSettings.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if USING_UNITY_XR_INTERACTION_TOOLKIT

namespace Lost.Haven
{
    using System;
    using UnityEngine;
    using UnityEngine.XR.Interaction.Toolkit;

    [Serializable]
    public class HavenClimbSettings
    {
        [Space]
        [SerializeField] private InteractionLayerMask interactionLayers = -1;
        [SerializeField] private GameObject customReticle;

        public void Apply(HavenClimbable climb)
        {
            climb.interactionLayers = this.interactionLayers;
            climb.customReticle = this.customReticle;
        }
    }
}

#endif
