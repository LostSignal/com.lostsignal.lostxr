//-----------------------------------------------------------------------
// <copyright file="HavenSocketEditor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost.Haven
{
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

            int foldoutId = this.target.GetInstanceID();

            this.Foldout("Advanced", () =>
            {
                this.DrawProperty("onlyAllowSpecificSocketTarget");
                this.DrawProperty("socketTargetName");
                this.DrawProperty("disableInteractorAndInteractableOnSocketed");
            });

            this.Foldout("Events", () =>
            {
                this.DrawProperty("onSocketed");
            });

            this.Foldout("Unity XRIT Events", () =>
            {
                this.DrawProperty("m_HoverEntered");
                this.DrawProperty("m_HoverExited");
                this.DrawProperty("m_SelectEntered");
                this.DrawProperty("m_SelectExited");
            });

            this.serializedObject.ApplyModifiedProperties();
        }
    }
}
