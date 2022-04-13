//-----------------------------------------------------------------------
// <copyright file="HavenSimpleSettings.cs" company="Lost Signal LLC">
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
    public class HavenSimpleSettings
    {
        [Space]
        [SerializeField] private InteractionLayerMask interactionLayers = -1;
        [SerializeField] private GameObject customReticle;
        [SerializeField] private InteractableSelectMode selectMode;

        public void Apply(HavenSimple simple)
        {
            simple.interactionLayers = this.interactionLayers;
            simple.customReticle = this.customReticle;
            simple.selectMode = this.selectMode;
        }
    }
}

#endif
