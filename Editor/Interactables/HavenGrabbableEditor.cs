//-----------------------------------------------------------------------
// <copyright file="HavenGrabbableEditor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost.Haven
{
    using Lost.EditorGrid;
    using UnityEditor;

    [CustomEditor(typeof(HavenGrabbable))]
    public class HavenGrabbableEditor : LostEditor
    {
        public override void OnInspectorGUI()
        {
            this.DrawProperty("havenGrabbableSettings");
            this.DrawProperty("isOffsetGrabbable");
            this.DrawProperty("m_AttachTransform");
            this.DrawProperty("m_Colliders");

            using (new FoldoutScope(this.target.GetInstanceID(), "Events", out bool visible))
            {
                if (visible)
                {
                    this.DrawProperty("onHoverStart");
                    this.DrawProperty("onHoverStop");

                    this.DrawProperty("onGrabStart");
                    this.DrawProperty("onGrabStop");

                    this.DrawProperty("onUseStart");
                    this.DrawProperty("onUseStop");
                }
            }

            this.serializedObject.ApplyModifiedProperties();
        }
    }
}
