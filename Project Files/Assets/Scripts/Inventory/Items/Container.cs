using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Container : MonoBehaviour
{
    //Class for item containers. It should be created on click, and it detects the container the player is hovering over, and loads all the corresponding information
    public ContainerType containertype = ContainerType.Normal;

    public ContainerFilled containerInfo = ContainerFilled.NotFilled;//Information about the type of Container
    private Transform containerTransform;//Container's transform

    //Centralized variable that gets and sets the container's transform as well as sets the item information
    public Transform container { get { return containerTransform; } set { containerTransform = value; } }

    //Determines the placement that the item contained has in the item list
    public int Placement { get { return container.gameObject.transform.GetSiblingIndex(); } }

    ////Enum that holds different types of containers.
    //NotFilled >>> Container is empty
    //FilledSameItemFull >>> Container holds the same type of item but the stack is full
    //FilledSameItemNotFull >>> Container holds the same type of item and the stack still can be filled 
    //FilledDifferentItem >>> Container holds a different type of item
    public enum ContainerFilled { NotFilled, FilledSameItemFull, FilledSameItemNotFull, FilledDifferentItem }
    public enum ContainerType { Normal,TradingSlot,EquipmentHatSlot,EquipmentTorsoSlot, EquipmentLegsSlot, EquipmentShoeSlot }

    //Checks if the container is an equipment container
    public bool IsEquipmentType()
    {
        if(containertype == ContainerType.EquipmentHatSlot || containertype == ContainerType.EquipmentTorsoSlot || containertype == ContainerType.EquipmentLegsSlot || containertype == ContainerType.EquipmentShoeSlot) return true;
        return false;
    }

    [HideInInspector] public List<InventoryManager.ItemContent> itemList;//List from which the container's items are generated

    //Method that gets information about the current container the user is hovering
    private Container GetContainer(Item item)
    {
        container = transform;
        containerInfo = ContainerFilled.NotFilled;

        //Fills information about the item contained 
        if (container.childCount > 0)
        {
            InventoryManager.ItemContent itemContent = GetItemReference(this);
            if (item.ItemName() == itemContent.item.Name)//If a NullReferenceException error points to this line it means the item that needs to be inserted/extracted is not part of the InventoryManager's itemList 
            {
                if (!item.StackableStatus() || itemContent.Size == itemContent.item.maxStack)
                {
                    containerInfo = ContainerFilled.FilledSameItemFull;
                }
                else containerInfo = ContainerFilled.FilledSameItemNotFull;
            }
            else containerInfo = ContainerFilled.FilledDifferentItem;
        }

        return this;
    }


    //Method that gets the container the user is hovering over
    public static Container GetHoveringContainer(Item item)
    {
        //Raycasts from cursor to the Canvas
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        //Searches for a container in all the UI objects it hit with the raycast
        if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.tag == "ItemContainer" && result.gameObject.activeSelf)
                {
                    //Gets the container data and sends it to the Item
                    Container c = result.gameObject.GetComponent<Container>();
                    c.GetContainer(item);
                    return c;
                }
            }
        }

        return null;

    }

    //Returns the first container avalabile from the list of sibling containers (when users buy items we want to fill the inventory's first empty spaces)
    public static int GetFirstContainerAvalabile(Container c)
    {
        foreach (Transform child in c.container.parent)
        {
            if (child.childCount == 0) return child.GetSiblingIndex();
        }

        return -1;
    }

    //Method that gets the item information about a specified GameObject (used for example for getting the item information of the current item)
    public static InventoryManager.ItemContent GetItemReference(Container c)
    {
        if (c == null) return null;
        if (c.container.childCount == 0) return null;

        return (c.container.GetChild(0).GetComponent<Item>().content);
    }


    //Methods that returns the inventory where the specified item is held
    public static InventoryManager GetInventory(GameObject gameObject)
    {
        return gameObject.GetComponentInParent<InventoryManager>();
    }

}
