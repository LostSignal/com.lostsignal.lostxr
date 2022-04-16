//-----------------------------------------------------------------------
// <copyright file="HavenClimbableEditor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost.Haven
{
    using UnityEditor;

    [CustomEditor(typeof(HavenClimbable))]
    public class HavenClimbableEditor : LostEditor
    {
        public override void OnInspectorGUI()
        {
            this.DrawProperty("havenClimbableSettings");
            this.DrawProperty("climbRigidbody");
            this.DrawProperty("m_Colliders");

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
