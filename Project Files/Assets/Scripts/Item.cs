using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Item : MonoBehaviour
{
    public string Name;
    public string description;

    [HideInInspector] public bool isTradable;
    [HideInInspector] public int price;

    public Sprite image;

    [HideInInspector] public bool hasDurability;
    [HideInInspector] public int maxDurability;
    [HideInInspector] public int currentDurability;

    private Image ImageComponent { get { return GetComponent<Image>();} }

    private void Update()
    {
        if(ImageComponent.sprite!=image) ImageComponent.sprite = image;
    }
}
