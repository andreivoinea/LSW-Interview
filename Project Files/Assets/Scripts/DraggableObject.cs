using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public abstract class DraggableObject : MonoBehaviour, IPointerClickHandler
{
    private bool isFollowing = false;
    private Transform initialContainer;

    private enum ContainerFilled { NotFilled, FilledSameItemFull, FilledSameItemNotFull, FilledDifferentItem }

    private class Container
    {
        public ContainerFilled containerInfo;
        private Transform containerTransform;
        public InventoryManager.ItemContent itemContent;

        public Transform container { get { return containerTransform; } set { containerTransform = value; SetItemContent(); } }

        public Container()
        {
            containerInfo = ContainerFilled.NotFilled;
            containerTransform = null;
            itemContent = null;
        }

        public void SetItemContent()
        {
            if (containerTransform.childCount == 0) return;
            itemContent = GetItemReference(containerTransform.GetChild(0).gameObject);
        }

        public int Placement { get { return container.gameObject.transform.GetSiblingIndex(); } }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isFollowing)
                InsertInContainer();
            else
                ExtractFromContainer();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isFollowing)
            {
                PlaceOne();
            }
            else
            {
                ExtractFromContainer(true);
            }
        }
    }
    public void Update()
    {
        FollowMouse();
        CheckEmptyStack();
    }
    private void FollowMouse()
    {
        if (!isFollowing) return;

        transform.position = Input.mousePosition;
    }

    public void ExtractFromContainer(bool half = false)
    {
        Container c = GetContainer();

        if (!half)
        {
            initialContainer = c.container;
            transform.SetParent(GameController.Instance.Inventory.InventorySlots);
        }
        else
        {
            int initSize = CurrentItemStack;
            GetItemReference(gameObject).Size = initSize % 2 == 0 ? initSize / 2 : initSize / 2 + 1;


            initialContainer = c.container;
            transform.SetParent(GameController.Instance.Inventory.InventorySlots);

            GameController.CreateItem(new InventoryManager.ItemContent(GameController.Instance.GetItem(GetItem()), c.Placement, initSize - GetItemReference(gameObject).Size), true);
        }

        isFollowing = true;
    }

    private void InsertInContainer(int size = -1, bool keep = false)
    {
        Container c = GetContainer();

        if (c.container == null) { ReturnToInitialContainer(); return; }

        if (size == -1)
        {
            if (c.containerInfo == ContainerFilled.NotFilled)
            {
                GetItemReference(gameObject).placement = c.Placement;
                transform.SetParent(c.container);
                transform.localPosition = new Vector3(50f, 0f, 0f);

                isFollowing = false;
            }
            else if (c.containerInfo == ContainerFilled.FilledDifferentItem)
            {
                GetItemReference(gameObject).placement = c.Placement;
                transform.SetParent(c.container);
                transform.localPosition = new Vector3(50f, 0f, 0f);

                c.itemContent.reference.GetComponent<Item>().ExtractFromContainer();

                isFollowing = false;
            }
            else if (c.containerInfo == ContainerFilled.FilledSameItemFull) return;
            else
            {
                if (CurrentItemStack + c.itemContent.Size > c.itemContent.item.maxStack)
                {
                    isFollowing = true;

                    GetItemReference(gameObject).Size -= c.itemContent.item.maxStack - c.itemContent.Size;
                    c.itemContent.Size = c.itemContent.item.maxStack;

                }
                else
                {
                    isFollowing = false;

                    c.itemContent.Size = CurrentItemStack + c.itemContent.Size;
                    DeleteItem(gameObject);
                }
            }
        }
        else
        {
            if (c.containerInfo == ContainerFilled.NotFilled && !keep)
            {
                GetItemReference(gameObject).placement = c.Placement;
                transform.SetParent(c.container);
                transform.localPosition = new Vector3(50f, 0f, 0f);

                isFollowing = false;
            }
            else
            {
                if (c.containerInfo == ContainerFilled.NotFilled && keep)
                {
                    GameController.CreateItem(new InventoryManager.ItemContent(GameController.Instance.GetItem(GetItem()), c.Placement, size), true);
                    GetItemReference(gameObject).Size = size;

                    isFollowing = true;
                }
                else
                {
                    if (CurrentItemStack + c.itemContent.Size > c.itemContent.item.maxStack)
                    {
                        isFollowing = true;

                        GetItemReference(gameObject).Size = c.itemContent.item.maxStack - c.itemContent.Size;
                        c.itemContent.Size = c.itemContent.item.maxStack;

                    }
                    else
                    {
                        isFollowing = false;

                        c.itemContent.Size += CurrentItemStack;
                        DeleteItem(gameObject);
                    }
                }

            }
        }
    }

    private Transform GetHoveringContainer()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

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

    private Container GetContainer()
    {
        Container c = new Container();
        c.container = GetHoveringContainer();
        if (c.container == null) return c;

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

    private void ReturnToInitialContainer()
    {
        transform.SetParent(initialContainer.gameObject.transform);
        transform.localPosition = new Vector3(50f, 0f, 0f);
        isFollowing = false;
    }

    private void PlaceOne()
    {
        Container c = GetContainer();

        if (c.containerInfo == ContainerFilled.FilledDifferentItem || c.containerInfo == ContainerFilled.FilledSameItemFull) return;

        InsertInContainer(1, true);
    }

    public static InventoryManager.ItemContent GetItemReference(GameObject comparingItem)
    {
        foreach (InventoryManager.ItemContent item in GameController.Instance.Inventory.prefabContents)
        {
            if (item.reference == comparingItem)
            {
                return item;
            }
        }

        return null;
    }

    abstract protected string ItemName();
    abstract protected bool StackableStatus();
    abstract protected Item GetItem();

    public int CurrentItemStack { get { return int.Parse(transform.GetChild(0).GetComponent<TextMeshProUGUI>().text); } set { transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = value.ToString();} }

    private void CheckEmptyStack()
    {
        if (CurrentItemStack == 0) DeleteItem(gameObject);
    }

    private void DeleteItem(GameObject item)
    {
        foreach (InventoryManager.ItemContent content in GameController.Instance.Inventory.prefabContents)
        {
            if (content.reference == item)
            {
                GameController.Instance.Inventory.prefabContents.Remove(content);
                break;
            }
        }

        Destroy(item);
    }


}
