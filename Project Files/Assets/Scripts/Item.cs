using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string Name;
    public string Description;

    [HideInInspector] public bool isTradable;
    [HideInInspector] public int price;

    public Image equippedImage;

    [HideInInspector] public bool hasDurability;
    [HideInInspector] public int maxDurability;
    [HideInInspector] public int currentDurability;
}
