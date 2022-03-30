//-----------------------------------------------------------------------
// <copyright file="HavenClimbSettingsObject.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


#if USING_UNITY_XR_INTERACTION_TOOLKIT

namespace Lost.Haven
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Lost/Haven/Haven Climb Settings")]
    public class HavenClimbSettingsObject : ScriptableObject
    {
#pragma warning disable 0649
        [ChildrenOnly]
        [Indent(1)]
        [SerializeField]
        private HavenClimbSettings settings;
#pragma warning restore 0649

        public void Apply(HavenClimbable climb)
        {
            this.settings.Apply(climb);
        }
    }
}

#endif
