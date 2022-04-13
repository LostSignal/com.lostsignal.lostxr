//-----------------------------------------------------------------------
// <copyright file="HavenSimpleEditor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost.Haven
{
    using Lost.EditorGrid;
    using UnityEditor;

    [CustomEditor(typeof(HavenSimple))]
    public class HavenSimpleEditor : LostEditor
    {
        public override void OnInspectorGUI()
        {
            this.DrawProperty("havenSimpleSettings");
            this.DrawProperty("disableRayGrab");
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
