using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StorageManager))]
public class StorageManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        StorageManager manager = target as StorageManager;

        SerializedObject serializedObject = new SerializedObject(manager);
        SerializedProperty list = serializedObject.FindProperty("allItems");

        EditorGUILayout.PropertyField(list, new GUIContent("All Prefabs"));
    }

}
