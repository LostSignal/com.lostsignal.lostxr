//-----------------------------------------------------------------------
// <copyright file="HavenClimbableSettings.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost.Haven
{
    using System;
    using UnityEngine;
    using UnityEngine.XR.Interaction.Toolkit;

    [Serializable]
    public class HavenClimbableSettings
    {
        [Space]
        [SerializeField] private InteractionLayerMask interactionLayers = -1;
        [SerializeField] private GameObject customReticle;
        [SerializeField] private InteractableSelectMode selectMode;

        public void Apply(HavenClimbable climb)
        {
            climb.interactionLayers = this.interactionLayers;
            climb.customReticle = this.customReticle;
            climb.selectMode = this.selectMode;
        }
    }
}

#endif
