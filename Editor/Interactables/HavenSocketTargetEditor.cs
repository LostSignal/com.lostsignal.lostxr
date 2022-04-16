//-----------------------------------------------------------------------
// <copyright file="HavenSocketTargetEditor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost.Haven
{
    using UnityEditor;

    [CustomEditor(typeof(HavenSocketTarget))]
    public class HavenSocketTargetEditor : LostEditor
    {
        public override void OnInspectorGUI()
        {
            this.DrawProperty("interactable");
            this.DrawProperty("interactableRigidbody");
            this.DrawProperty("socketTargetName");

            this.Foldout("Events", () =>
            {
                this.DrawProperty("onSocketed");
            });

            this.serializedObject.ApplyModifiedProperties();
        }

    }
}
