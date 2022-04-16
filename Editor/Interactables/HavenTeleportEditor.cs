//-----------------------------------------------------------------------
// <copyright file="HavenTeleportEditor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost.Haven
{
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

            this.Foldout("Events", () =>
            {
                this.DrawProperty("onHoverStart");
                this.DrawProperty("onHoverStop");
                this.DrawProperty("onTeleport");
            });

            this.Foldout("Unity XRIT Events", () =>
            {
                this.DrawProperty("m_HoverEntered");
                this.DrawProperty("m_HoverExited");
                this.DrawProperty("m_SelectEntered");
                this.DrawProperty("m_SelectExited");
                this.DrawProperty("m_Activated");
                this.DrawProperty("m_Deactivated");
            });

            this.serializedObject.ApplyModifiedProperties();
        }
    }
}
