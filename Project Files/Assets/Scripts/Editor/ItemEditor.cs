using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Item item = target as Item;

        SerializedObject serializedObject = new SerializedObject(item);

        SerializedProperty isTradable = serializedObject.FindProperty("isTradable");
        SerializedProperty price = serializedObject.FindProperty("price");

        isTradable.boolValue = EditorGUILayout.Toggle("Is Tradable", isTradable.boolValue);
        if (isTradable.boolValue) price.intValue = EditorGUILayout.IntField( "Price", price.intValue);



        SerializedProperty hasDurability = serializedObject.FindProperty("hasDurability");
        SerializedProperty maxDurability = serializedObject.FindProperty("maxDurability");
        SerializedProperty currentDurability = serializedObject.FindProperty("currentDurability");

        hasDurability.boolValue = EditorGUILayout.Toggle("Has Durability", hasDurability.boolValue);
        if (hasDurability.boolValue)
        {
            maxDurability.intValue = EditorGUILayout.IntField("Max Durability", maxDurability.intValue);
            currentDurability.intValue = EditorGUILayout.IntField("Current Durability", currentDurability.intValue);
        }



        SerializedProperty isStackable = serializedObject.FindProperty("isStackable");
        SerializedProperty maxStack = serializedObject.FindProperty("maxStack");

        isStackable.boolValue = EditorGUILayout.Toggle("Is Stackable", isStackable.boolValue);
        if (isStackable.boolValue) maxStack.intValue = EditorGUILayout.IntField("Max Stack Size", maxStack.intValue);


        serializedObject.ApplyModifiedProperties();
    }


}
