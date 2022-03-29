//-----------------------------------------------------------------------
// <copyright file="LostEditor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public class LostEditor : Editor
    {
        private Dictionary<string, SerializedProperty> serializedProperties = new();

        protected virtual void OnEnable() => this.serializedProperties.Clear();

        protected virtual void OnDisable() => this.serializedProperties.Clear();

        protected void DrawProperty(string propertyName)
        {
            if (this.serializedProperties.TryGetValue(propertyName, out SerializedProperty prop) == false)
            {
                prop = this.serializedObject.FindProperty(propertyName);
                this.serializedProperties.Add(propertyName, prop);
            }
            
            EditorGUILayout.PropertyField(prop);
        }
    }
}
