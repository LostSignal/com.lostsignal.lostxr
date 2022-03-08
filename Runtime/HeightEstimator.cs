//-----------------------------------------------------------------------
// <copyright file="HeightEstimator.cs" company="DefaultCompany">
//     Copyright (c) DefaultCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost.XR
{
    using UnityEngine;

    public class HeightEstimator : MonoBehaviour
    {
        public Transform leftHand;
        public Transform rightHand;

        private float maxDistance;

        private void Update()
        {
            float distance = Vector3.Distance(leftHand.position, rightHand.position);

            if (distance > this.maxDistance)
            {
                this.maxDistance = distance;
                Debug.Log(this.maxDistance);
            }
        }
    }
}

#endif
