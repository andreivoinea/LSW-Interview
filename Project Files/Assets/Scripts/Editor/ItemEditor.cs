using System.Collections;
using System.Collections.Generic;
using System;
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

        EditorGUILayout.Space();

        SerializedProperty hasDurability = serializedObject.FindProperty("hasDurability");
        SerializedProperty maxDurability = serializedObject.FindProperty("maxDurability");
        SerializedProperty currentDurability = serializedObject.FindProperty("currentDurability");

        hasDurability.boolValue = EditorGUILayout.Toggle("Has Durability", hasDurability.boolValue);
        if (hasDurability.boolValue)
        {
            maxDurability.intValue = EditorGUILayout.IntField("Max Durability", maxDurability.intValue);
            currentDurability.intValue = EditorGUILayout.IntField("Current Durability", currentDurability.intValue);
        }

        EditorGUILayout.Space();

        SerializedProperty isStackable = serializedObject.FindProperty("isStackable");
        SerializedProperty maxStack = serializedObject.FindProperty("maxStack");

        isStackable.boolValue = EditorGUILayout.Toggle("Is Stackable", isStackable.boolValue);
        if (isStackable.boolValue) maxStack.intValue = EditorGUILayout.IntField("Max Stack Size", maxStack.intValue);

        EditorGUILayout.Space();

        SerializedProperty isEquippable = serializedObject.FindProperty("isEquippable");
        SerializedProperty inGameTexture = serializedObject.FindProperty("inGameTexture");
        SerializedProperty itemType = serializedObject.FindProperty("itemType");

        isEquippable.boolValue = EditorGUILayout.Toggle("IsEquippable", isEquippable.boolValue);
        if (isEquippable.boolValue)
        {
            inGameTexture.objectReferenceValue = (Sprite)EditorGUILayout.ObjectField("Eqquiped Image", inGameTexture.objectReferenceValue, typeof(Sprite), false);
            Item.EquipableItemType current;
            current = (Item.EquipableItemType)itemType.enumValueIndex;
            itemType.enumValueIndex = Convert.ToInt32(EditorGUILayout.EnumPopup("Item type", item.itemType));
        }


        serializedObject.ApplyModifiedProperties();
    }


}
