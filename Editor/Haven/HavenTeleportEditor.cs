#pragma warning disable

//-----------------------------------------------------------------------
// <copyright file="HavenTeleportEditor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
            this.DrawProperty("m_InteractionLayerMask");
            this.DrawProperty("m_CustomReticle");
            this.DrawProperty("type");
            this.DrawProperty("anchorTransform");
            this.DrawProperty("matchAnchorOrientation");
            this.DrawProperty("m_Colliders");

            using (new FoldoutScope(846589, "Events", out bool visible))
            {
                if (visible)
                {
                    this.DrawProperty("onHoverStart");
                    this.DrawProperty("onHoverEnd");
                }
            }
        }
    }
}
