//-----------------------------------------------------------------------
// <copyright file="HavenGrabbableSettingsObject.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost.Haven
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Lost/Haven/Haven Grabbable Settings")]
    public class HavenGrabbableSettingsObject : ScriptableObject
    {
        #pragma warning disable 0649
        //[Indent(-1)]  // TODO [bgish]: Implement
        //[HideLabel]   // TODO [bgish]: Implement
        [SerializeField] 
        private HavenGrabbableSettings settings;
        #pragma warning restore 0649

        public void Apply(HavenGrabbable grabable)
        {
            this.settings.Apply(grabable);
        }
    }
}

#endif
