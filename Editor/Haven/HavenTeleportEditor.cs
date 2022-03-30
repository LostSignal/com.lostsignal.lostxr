#pragma warning disable

//-----------------------------------------------------------------------
// <copyright file="HavenTeleportEditor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if USING_UNITY_XR_INTERACTION_TOOLKIT

namespace Lost.Haven
{
    using Lost.EditorGrid;
    using Lost.XR;
    using UnityEditor;

    [CustomEditor(typeof(HavenTeleport)), CanEditMultipleObjects]
    public class HavenTeleportEditor : LostEditor
    {
        public override void OnInspectorGUI()
        {
            this.DrawProperty("m_CustomReticle");
            this.DrawProperty("type");
            this.DrawProperty("anchorTransform");
            this.DrawProperty("matchAnchorOrientation");
            this.DrawProperty("m_Colliders");

            using (new FoldoutScope(this.target.GetInstanceID(), "Events", out bool visible))
            {
                if (visible)
                {
                    this.DrawProperty("onHoverStart");
                    this.DrawProperty("onHoverEnd");
                }
            }

            this.serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif
