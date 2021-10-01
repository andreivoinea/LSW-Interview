using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[System.Serializable]
public class Item : DraggableObject
{
    //This Script can be attached to Items. It only holds Item data and it extends DraggableObject. All item data manipulation methods will be created and used in DraggableObject. 
    //It also extends a few abstract methods that allow DraggableObject to access Item variables indirectly
    //
    
    public string Name;//Item Name
    public string description;//Item Description

    [HideInInspector] public bool isTradable;//Is Item Tradable
    [HideInInspector] public int price;//Item Price

    public Sprite image;//Item Sprite

    [HideInInspector] public bool hasDurability;//Does Item have durability
    [HideInInspector] public int maxDurability;//Item Max Durability
    [HideInInspector] public int currentDurability;//Item Current Durability

    [HideInInspector] public bool isStackable;//Is Item Stackable
    [HideInInspector] public int maxStack;//Item Max Stacking number


    private Image ImageComponent { get { return GetComponent<Image>();} }//Image Component of the Item

    private void Awake()
    {
        if(isStackable) transform.GetChild(0).gameObject.SetActive(true);//If Item can stack turn on the corresponding UI

        if (ImageComponent.sprite != image) ImageComponent.sprite = image;//Sets the sprite to the Image Component
    }
    private new void Update()
    {
        base.Update();//Uses DraggableObject Update before it's own Update method

        if(ImageComponent.sprite!=image) ImageComponent.sprite = image;//Sets the sprite to the Image Component
    }

    //Gets Item Name for usage in DraggableObject
    public override string ItemName()
    {
        return Name;
    }

    //Checks if item is stackable for usage in DraggableObject
    public override bool StackableStatus()
    {
        return isStackable;
    }

    //Returns Item Component for usage in DraggableObject
    public override Item GetItem()
    {
        return this;
    }

}
