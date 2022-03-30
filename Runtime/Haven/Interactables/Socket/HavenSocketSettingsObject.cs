//-----------------------------------------------------------------------
// <copyright file="HavenSocketSettingsObject.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if USING_UNITY_XR_INTERACTION_TOOLKIT

namespace Lost.Haven
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Lost/Haven/Haven Socket Settings")]
    public class HavenSocketSettingsObject : ScriptableObject
    {
#pragma warning disable 0649
        [ChildrenOnly]
        [Indent(1)]
        [SerializeField]
        private HavenSocketSettings settings;
#pragma warning restore 0649

        public void Apply(HavenSocket socket)
        {
            this.settings.Apply(socket);
        }
    }
}

#endif
