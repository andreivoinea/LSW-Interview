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

        item.isTradable = EditorGUILayout.Toggle("Is Tradable", item.isTradable);
        if (item.isTradable) item.price = EditorGUILayout.IntField("Price", item.price);

        item.hasDurability = EditorGUILayout.Toggle("Has Durability", item.hasDurability);
        if (item.hasDurability)
        {
            item.maxDurability = EditorGUILayout.IntField("Max Durability", item.maxDurability);
            item.currentDurability = EditorGUILayout.IntField("Current Durability", item.currentDurability);
        }

        item.isStackable = EditorGUILayout.Toggle("Is Stackable", item.isStackable);
        if (item.isStackable) item.maxStack = EditorGUILayout.IntField("Max Stack Size", item.maxStack);


    }


}
