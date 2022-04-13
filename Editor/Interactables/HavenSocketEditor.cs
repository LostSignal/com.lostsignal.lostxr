//-----------------------------------------------------------------------
// <copyright file="HavenSocketEditor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost.Haven
{
    using Lost.EditorGrid;
    using UnityEditor;

    [CustomEditor(typeof(HavenSocket))]
    public class HavenSocketEditor : LostEditor
    {
        public override void OnInspectorGUI()
        {
            this.DrawProperty("havenSocketSettings");
            this.DrawProperty("m_SocketActive");
            this.DrawProperty("m_StartingSelectedInteractable");
            this.DrawProperty("m_AttachTransform");
            this.DrawProperty("onlyAllowSpecificSocketTarget");
            this.DrawProperty("socketTargetName");

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
