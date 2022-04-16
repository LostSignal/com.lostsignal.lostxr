//-----------------------------------------------------------------------
// <copyright file="HavenGrabbableEditor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost.Haven
{
    using UnityEditor;

    [CustomEditor(typeof(HavenGrabbable))]
    public class HavenGrabbableEditor : LostEditor
    {
        public override void OnInspectorGUI()
        {
            this.DrawProperty("havenGrabbableSettings");
            this.DrawProperty("isOffsetGrabbable");
            this.DrawProperty("disableRayGrab");
            this.DrawProperty("m_AttachTransform");
            this.DrawProperty("m_Colliders");

            this.Foldout("Events", () =>
            {
                this.DrawProperty("onHoverStart");
                this.DrawProperty("onHoverStop");

                this.DrawProperty("onGrabStart");
                this.DrawProperty("onGrabStop");

                this.DrawProperty("onUseStart");
                this.DrawProperty("onUseStop");
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
