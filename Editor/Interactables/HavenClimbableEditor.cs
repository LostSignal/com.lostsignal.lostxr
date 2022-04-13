//-----------------------------------------------------------------------
// <copyright file="HavenClimbableEditor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost.Haven
{
    using Lost.EditorGrid;
    using UnityEditor;

    [CustomEditor(typeof(HavenClimbable))]
    public class HavenClimbableEditor : LostEditor
    {
        public override void OnInspectorGUI()
        {
            this.DrawProperty("havenClimbableSettings");
            this.DrawProperty("climbRigidbody");
            this.DrawProperty("m_Colliders");

            using (new FoldoutScope(this.target.GetInstanceID(), "Events", out bool visible))
            {
                if (visible)
                {
                    this.DrawProperty("m_HoverEntered");
                    this.DrawProperty("m_HoverExited");
                    this.DrawProperty("m_SelectEntered");
                    this.DrawProperty("m_SelectExited");
                }
            }

            this.serializedObject.ApplyModifiedProperties();
        }
    }
}
