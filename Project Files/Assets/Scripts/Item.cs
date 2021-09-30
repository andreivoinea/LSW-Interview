using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[System.Serializable]
public class Item : DraggableObject
{
    public string Name;
    public string description;

    [HideInInspector] public bool isTradable;
    [HideInInspector] public int price;

    public Sprite image;

    [HideInInspector] public bool hasDurability;
    [HideInInspector] public int maxDurability;
    [HideInInspector] public int currentDurability;

    [HideInInspector] public bool isStackable;
    [HideInInspector] public int maxStack;


    private Image ImageComponent { get { return GetComponent<Image>();} }

    private void Awake()
    {
        if(isStackable)
        transform.GetChild(0).gameObject.SetActive(true);

        if (ImageComponent.sprite != image) ImageComponent.sprite = image;
    }
    private new void Update()
    {
        base.Update();
        if(ImageComponent.sprite!=image) ImageComponent.sprite = image;
    }

    protected override string ItemName()
    {
        return Name;
    }

    protected override bool StackableStatus()
    {
        return isStackable;
    }

    protected override Item GetItem()
    {
        return this;
    }

}
