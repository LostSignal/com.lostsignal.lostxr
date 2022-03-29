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
            // base.OnInspectorGUI();

            this.DrawProperty("havenGrabbableSettings");

            this.DrawProperty("Interaction Layer Mask");
            this.DrawProperty("Custom Reticle");
            this.DrawProperty("Colliders");
            this.DrawProperty("Select Mode");  // To Do - Actually move this to setting object
            this.DrawProperty("Attach Transform");
            this.DrawProperty("Is Offset Grabbable");

            // Make Events Foldout
            this.DrawProperty("Hover Start/Stop");
            this.DrawProperty("Grab Start/Stop");
            this.DrawProperty("Use Start/Stop");

            // Null out InteractionManager
            // Add colliders if empty
            // Make colliders to the right layer

            // Make an Advanced Section?  All
        }
    }
}
