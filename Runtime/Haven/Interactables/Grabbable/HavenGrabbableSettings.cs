//-----------------------------------------------------------------------
// <copyright file="HavenGrabbableSettings.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost.Haven
{
    using System;
    using UnityEngine;

    [Serializable]
    public class HavenGrabbableSettings
    {
        [SerializeField] private UnityEngine.XR.Interaction.Toolkit.XRBaseInteractable.MovementType movementType;
    
        // Position
        [SerializeField] private bool trackPosition;
        [SerializeField] private bool smoothPosition;
        [SerializeField] private float smoothPositionAmount;  // 0 to 20
        [SerializeField] private float tightenPosition; // 0 to 1
    
        // Rotation
        [SerializeField] private bool trackRotation;
        [SerializeField] private bool smoothRotation;
    
        // Thowing
        [SerializeField] private bool throwOnDetach;
        [SerializeField] private float throwSmoothingDuration;
        [SerializeField] private AnimationCurve throwSmoothingCurve;
        [SerializeField] private float throwVelocityScale;
        [SerializeField] private float throwAngularVelocityScale;
    
        // Attaching
        [SerializeField] private bool retainTransformParent;
        [SerializeField] private bool forceGravityOnDetach;
        [SerializeField] private float attachEaseInTime;
    
        public void Apply(HavenGrabbable grabbable)
        {
            grabbable.movementType = this.movementType;
    
            // Position
            grabbable.trackPosition = this.trackPosition;
            grabbable.smoothPosition = this.smoothPosition;
            grabbable.smoothPositionAmount = this.smoothPositionAmount;
            grabbable.tightenPosition = this.tightenPosition;
    
            // Rotation
            grabbable.trackRotation = this.trackRotation;
            grabbable.smoothRotation = this.smoothRotation;
    
            // Throwing
            grabbable.throwOnDetach = this.throwOnDetach;
            grabbable.throwSmoothingDuration = this.throwSmoothingDuration;
            grabbable.throwSmoothingCurve = this.throwSmoothingCurve;
            grabbable.throwVelocityScale = this.throwVelocityScale;
            grabbable.throwAngularVelocityScale = this.throwAngularVelocityScale;
    
            // Attaching
            grabbable.retainTransformParent = this.retainTransformParent;
            grabbable.forceGravityOnDetach = this.forceGravityOnDetach;
            grabbable.attachEaseInTime = this.attachEaseInTime;
        }
    }
}

#endif
