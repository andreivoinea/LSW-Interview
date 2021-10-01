using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Linq;

public abstract class DraggableObject : MonoBehaviour, IPointerDownHandler
{
    //Base Script for the Item Class. All items extend this class and this class contains all methods that users and game logic can interact with items
    //
    //

    private bool isFollowing = false;//Variable that represents the item following the mouse cursor
    private Transform initialContainer;//Initial Container used when an item needs to go back to it's original placed, because of an invalid position

    public InventoryManager.ItemContent content;

    //Method that handles the user click interactions
    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left) // Left Click
        {
            if (isFollowing)
                InsertInContainer();//Put item in container
            else
                ExtractItem();//Pick item from container
        }
        else if (eventData.button == PointerEventData.InputButton.Right)//Right Click
        {
            if (isFollowing)
            {
                PlaceOne();//Put only 1 item in container
            }
            else
            {
                ExtractItem(true);//Pick only half the items from container
            }
        }
    }

    public void Update()
    {
        FollowMouse();//Makes item follow the mouse cursor
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
        Container c = Container.GetHoveringContainer(GetItem());
        Container init = GetInitialContainer();

        InventoryManager.ItemContent containerContent = Container.GetItemReference(c);

        if (c == null) { ReturnToInitialContainer(); return; }//If there was a bad click return to the initial container

                c.itemList.Add(content);
            if (size == -1)
                init.itemList.Remove(content);

        switch (c.containerInfo)
        {
            case Container.ContainerFilled.NotFilled://If the container is empty
                if (!keep)//If we don't want to keep the item, we just insert it in the container
                {
                    content.placement = c.Placement;
                    transform.SetParent(c.container);
                    transform.localPosition = new Vector3(50f,50f,0f);//Accounts for the Item Pivot

                    isFollowing = false;
                    return;
                }
                else//If we want to keep the item, we create a new Item with the amount we want to place and remove that amount from the current item stack
                {
                    GameController.CreateItem(new InventoryManager.ItemContent(GameController.Instance.GetItem(GetItem()), c.Placement, size), c.itemList, c.container.parent, true);

                    content.Size -= size;

                    if (content.size <= 0) DeleteItem(gameObject,init.itemList);

                    isFollowing = true;
                    return;
                }

            case Container.ContainerFilled.FilledDifferentItem://If the container is filled with a different item
                if (size == -1)//If we want to place all the current stack, we are going to extract the existing stack
                {
                    content.placement = c.Placement;
                    transform.SetParent(c.container);
                    transform.localPosition = new Vector3(50f, 50f, 0f);//Accounts for the Item Pivot

                    Container.GetItemReference(c).reference.GetComponent<Item>().ExtractItem();

                    isFollowing = false;
                }
                return;//If we want to place a specific amount from the stack, it will not place it, instead keep it selected for the user

            case Container.ContainerFilled.FilledSameItemFull://If the container is filled with the same item and it's full then we don't do any action and await for the user to do something else
                return;

            case Container.ContainerFilled.FilledSameItemNotFull://If the container is filled with the same item and it's not full
                if (size == -1) size = CurrentItemStack;//Set the size to insert
                if (size + content.Size > content.item.maxStack)//If the amount we have add with the existing amount would exceed a stack size, we only input items until we fill the stack and keep the item selected for the user
                {
                    isFollowing = true;

                    content.Size -= content.item.maxStack - containerContent.Size;
                    if (content.size <= 0) DeleteItem(gameObject, init.itemList);

                    containerContent.Size = content.item.maxStack;
                    return;
                }
                else//If the amount we have add with the existing amount do not exceed the stack size, we add them together
                {
                    Container.GetItemReference(c).Size += size;

                    if (keep)
                    {
                        content.Size -= size;
                        if (content.size <= 0) DeleteItem(gameObject, init.itemList);
                    }
                    else
                        DeleteItem(gameObject, c.itemList);

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
        Container c = Container.GetHoveringContainer(GetItem());

        if (c.containerInfo == Container.ContainerFilled.FilledDifferentItem || c.containerInfo == Container.ContainerFilled.FilledSameItemFull) return;

        InsertInContainer(1, true);//Inserts only one item with the intention of keeping the item if the stack is full already
    }

    public void ExtractItem(bool half = false)
    {
        Container c = Container.GetHoveringContainer(GetItem());

        Container.GetItemReference(c).reference.GetComponent<Item>().ExtractFromContainer(c, half);

    }

    //Method that extracts items from a container
    public void ExtractFromContainer(Container c,bool half = false)//Argument half is set to true if the user wants to extract only a half of the items
    {
        if(c == null) return;
        initialContainer = c.container;//Sets the initial container

        Transform containerParent;

        if (c.isTradingSlot)
        {
            containerParent = c.container.parent.parent.parent;
        }
        else containerParent = c.container.parent;

        if (!half || CurrentItemStack == 1)//Full extraction when half is set to false or the container only has 1 item
            transform.SetParent(containerParent);//Extracts Item        
        else //Half Extraction
        {
            int initSize = CurrentItemStack;
            content.Size = initSize % 2 == 0 ? initSize / 2 : initSize / 2 + 1;//calculates the new stack size

            transform.SetParent(containerParent);//Extracts Item

            GameController.CreateItem(new InventoryManager.ItemContent(GameController.Instance.GetItem(GetItem()), c.Placement, initSize - content.Size), c.itemList, c.container.parent, true);//Creates new Item with the remaining stack size
        }

        isFollowing = true;
    }

    private Container GetInitialContainer()
    {
        return initialContainer.GetComponent<Container>();
    }

    //Method that returns item to the initial container in case of a bad placement by the user
    private void ReturnToInitialContainer()
    {
        transform.SetParent(initialContainer.gameObject.transform);
        transform.localPosition = new Vector3(50f, 50f, 0f); //Accounts for the Item Pivot
        isFollowing = false;
    }



    //Method that is able to fully delete an item instance, both from the scene and the item list
    private void DeleteItem(GameObject item,List<InventoryManager.ItemContent> list)
    {
        Debug.Log(item.name);
       

        foreach (InventoryManager.ItemContent content in list)
        {
            if (content.reference == item)
            {
                list.Remove(content);
                break;
            }
        }

        Destroy(item);
    }

    //Method that modifies the stack UI for the item
    public int CurrentItemStack { get { return int.Parse(transform.GetChild(0).GetComponent<TextMeshProUGUI>().text); } set { transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = value.ToString(); } }

    //Abstract Methods that are overrided in <Item>
    abstract public string ItemName();
    abstract public bool StackableStatus();
    abstract public Item GetItem();

}
