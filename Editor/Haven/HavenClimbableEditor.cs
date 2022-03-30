//-----------------------------------------------------------------------
// <copyright file="HavenClimbableEditor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if USING_UNITY_XR_INTERACTION_TOOLKIT

namespace Lost.Haven
{
    using Lost.EditorGrid;
    using UnityEditor;

    [CustomEditor(typeof(HavenClimbable))]
    public class HavenClimbableEditor : LostEditor
    {
        public override void OnInspectorGUI()
        {
            this.DrawProperty("havenClimbSettings");
            this.DrawProperty("climbRigidbody");
            this.DrawProperty("m_Colliders");

            using (new FoldoutScope(this.target.GetInstanceID(), "Events", out bool visible))
            {
                if (visible)
                {
                    this.DrawProperty("m_HoverEntered");
                    this.DrawProperty("m_HoverExited");
                    this.DrawProperty("m_SelectEntered");
                }
            }

            this.serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif
