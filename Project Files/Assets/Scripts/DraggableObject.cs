using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public abstract class DraggableObject : MonoBehaviour, IPointerClickHandler
{
    //Base Script for the Item Class. All items extend this class and this class contains all methods that users and game logic can interact with items
    //
    //

    private bool isFollowing = false;//Variable that represents the item following the mouse cursor
    private Transform initialContainer;//Initial Container used when an item needs to go back to it's original placed, because of an invalid position

    //Class for item containers. It should be created on click, and it detects the container the player is hovering over, and loads all the corresponding information
    private class Container
    {
        public ContainerFilled containerInfo;//Information about the type of Container
        private Transform containerTransform;//Container's transform
        public InventoryManager.ItemContent itemContent;//Information about the item inside the container

        //Centralized variable that gets and sets the container's transform as well as sets the item information
        public Transform container { get { return containerTransform; } set { containerTransform = value; SetItemContent(); } }

        //Constructor for an empty container
        public Container()
        {
            containerInfo = ContainerFilled.NotFilled;
            containerTransform = null;
            itemContent = null;
        }

        //Loads the information about the item contained
        public void SetItemContent()
        {
            if (containerTransform.childCount == 0) return;
            itemContent = GetItemReference(containerTransform.GetChild(0).gameObject);
        }

        //Determines the placement that the item contained has in the item list
        public int Placement { get { return container.gameObject.transform.GetSiblingIndex(); } }
    }

    ////Enum that holds different types of containers.
    //NotFilled >>> Container is empty
    //FilledSameItemFull >>> Container holds the same type of item but the stack is full
    //FilledSameItemNotFull >>> Container holds the same type of item and the stack still can be filled 
    //FilledDifferentItem >>> Container holds a different type of item
    private enum ContainerFilled { NotFilled, FilledSameItemFull, FilledSameItemNotFull, FilledDifferentItem }

    //Method that gets information about the current container the user is hovering
    private Container GetContainer()
    {
        Container c = new Container();
        c.container = GetHoveringContainer();//Gets the type of container

        if (c.container == null) return c;

        //Fills information about the item contained 
        if (c.container.childCount > 0)
        {
            if (ItemName() == c.itemContent.item.Name)
            {
                if (!StackableStatus() || c.itemContent.Size == c.itemContent.item.maxStack)
                {
                    c.containerInfo = ContainerFilled.FilledSameItemFull;
                }
                else c.containerInfo = ContainerFilled.FilledSameItemNotFull;
            }
            else c.containerInfo = ContainerFilled.FilledDifferentItem;
        }

        return c;
    }

    //Method that gets the container the user is hovering over
    private Transform GetHoveringContainer()
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
                if (result.gameObject.tag == "ItemContainer")
                {
                    return result.gameObject.transform;
                }
            }
        }

        return null;
    }

    //Method that handles the user click interactions
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) // Left Click
        {
            if (isFollowing)
                InsertInContainer();//Put item in container
            else
                ExtractFromContainer();//Pick item from container
        }
        else if (eventData.button == PointerEventData.InputButton.Right)//Right Click
        {
            if (isFollowing)
            {
                PlaceOne();//Put only 1 item in container
            }
            else
            {
                ExtractFromContainer(true);//Pick only half the items from container
            }
        }
    }

    public void Update()
    {
        FollowMouse();//Makes item follow the mouse cursor
        CheckEmptyStack();//Checks the stack size of the item
    }

    //Method that makes the Item follow the cursor
    private void FollowMouse()
    {
        if (!isFollowing) return;

        transform.position = Input.mousePosition;
    }

    //Method that inserts items in a container
    private void InsertInContainer(int size = -1, bool keep = false)
    {
        Container c = GetContainer();

        if (c.container == null) { ReturnToInitialContainer(); return; }//If there was a bad click return to the initial container

        switch (c.containerInfo)
        {
            case ContainerFilled.NotFilled://If the container is empty
                if (!keep)//If we don't want to keep the item, we just insert it in the container
                {
                    GetItemReference(gameObject).placement = c.Placement;
                    transform.SetParent(c.container);
                    transform.localPosition = new Vector3(50f, 0f, 0f);

                    isFollowing = false;
                    return;
                }
                else//If we want to keep the item, we create a new Item with the amount we want to place and remove that amount from the current item stack
                {
                    GameController.CreateItem(new InventoryManager.ItemContent(GameController.Instance.GetItem(GetItem()), c.Placement, size), true);
                    GetItemReference(gameObject).Size -= size;

                    isFollowing = true;
                    return;
                }

            case ContainerFilled.FilledDifferentItem://If the container is filled with a different item
                if (size == -1)//If we want to place all the current stack, we are going to extract the existing stack
                {
                    GetItemReference(gameObject).placement = c.Placement;
                    transform.SetParent(c.container);
                    transform.localPosition = new Vector3(50f, 0f, 0f);

                    c.itemContent.reference.GetComponent<Item>().ExtractFromContainer();

                    isFollowing = false;
                }
                return;//If we want to place a specific amount from the stack, it will not place it, instead keep it selected for the user

            case ContainerFilled.FilledSameItemFull://If the container is filled with the same item and it's full then we don't do any action and await for the user to do something else
                return;

            case ContainerFilled.FilledSameItemNotFull://If the container is filled with the same item and it's not full
                if (size == -1) size = CurrentItemStack;//Set the size to insert
                if (size + c.itemContent.Size > c.itemContent.item.maxStack)//If the amount we have add with the existing amount would exceed a stack size, we only input items until we fill the stack and keep the item selected for the user
                {
                    isFollowing = true;

                    GetItemReference(gameObject).Size -= c.itemContent.item.maxStack - c.itemContent.Size;
                    c.itemContent.Size = c.itemContent.item.maxStack;
                    return;
                }
                else//If the amount we have add with the existing amount do not exceed the stack size, we add them together
                {
                    c.itemContent.Size += size;

                    if (keep) GetItemReference(gameObject).Size -= size;
                    else
                        DeleteItem(gameObject);

                    isFollowing = true;

                    return;
                }
            default:
                ReturnToInitialContainer(); return;//It defaults to returning to the initial container
        }
        
    }

    //Method that places only one item inside a container
    private void PlaceOne()
    {
        Container c = GetContainer();

        if (c.containerInfo == ContainerFilled.FilledDifferentItem || c.containerInfo == ContainerFilled.FilledSameItemFull) return;

        InsertInContainer(1, true);//Inserts only one item with the intention of keeping the item if the stack is full already
    }

    //Method that extracts items from a container
    public void ExtractFromContainer(bool half = false)//Argument half is set to true if the user wants to extract only a half of the items
    {
        Container c = GetContainer();
        initialContainer = c.container;//Sets the initial container

        if (!half || CurrentItemStack==1)//Full extraction when half is set to false or the container only has 1 item
            transform.SetParent(GameController.Instance.Inventory.InventorySlots);//Extracts Item
        else //Half Extraction
        {
            int initSize = CurrentItemStack;
            GetItemReference(gameObject).Size = initSize % 2 == 0 ? initSize / 2 : initSize / 2 + 1;//calculates the new stack size

            transform.SetParent(GameController.Instance.Inventory.InventorySlots);//Extracts Item

            GameController.CreateItem(new InventoryManager.ItemContent(GameController.Instance.GetItem(GetItem()), c.Placement, initSize - GetItemReference(gameObject).Size), true);//Creates new Item with the remaining stack size
        }

        isFollowing = true;
    }

    //Method that returns item to the initial container in case of a bad placement by the user
    private void ReturnToInitialContainer()
    {
        transform.SetParent(initialContainer.gameObject.transform);
        transform.localPosition = new Vector3(50f, 0f, 0f);
        isFollowing = false;
    }

    //Method that gets the item information about a specified GameObject (used for example for getting the item information of the current item)
    public static InventoryManager.ItemContent GetItemReference(GameObject comparingItem)
    {
        foreach (InventoryManager.ItemContent item in GameController.Instance.Inventory.itemList)
        {
            if (item.reference == comparingItem)
            {
                return item;
            }
        }

        return null;
    }

    //Method that modifies the stack UI for the item
    public int CurrentItemStack { get { return int.Parse(transform.GetChild(0).GetComponent<TextMeshProUGUI>().text); } set { transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = value.ToString();} }

    //Method that deletes the item if the stack is empty
    private void CheckEmptyStack()
    {
        if (CurrentItemStack == 0) DeleteItem(gameObject);
    }

    //Method that is able to fully delete an item instance, both from the scene and the item list
    private void DeleteItem(GameObject item)
    {
        foreach (InventoryManager.ItemContent content in GameController.Instance.Inventory.itemList)
        {
            if (content.reference == item)
            {
                GameController.Instance.Inventory.itemList.Remove(content);
                break;
            }
        }

        Destroy(item);
    }
    
    //Abstract Methods that are overloaded in <Item>
    abstract protected string ItemName();
    abstract protected bool StackableStatus();
    abstract protected Item GetItem();

}
