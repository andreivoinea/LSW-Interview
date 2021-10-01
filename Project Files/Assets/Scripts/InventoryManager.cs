using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class InventoryManager : MonoBehaviour
{
    //This Script Handles all inventory related queries
    //
    //

    //The ItemContent class holds more information than a regular Item, such as current stack size, placement in the inventory slot and the prefab from which the  item is created
    [System.Serializable]
    public class ItemContent {
        [SerializeField] public Item item;//Information about the prefab
        [SerializeField] public GameObject reference;//The Item GameObject
        public int placement;//Placement in the inventory slot
        public int size;//Current stack size
        public int Size { get { return size; } set { size = value; reference.GetComponent<Item>().CurrentItemStack = value; } }//Gets and sets the current stack size,whilst also changing the corresponding UI

        public ItemContent(Item inputItem, int position, int currentSize)//Construnctor for new Items
        {
            item = inputItem;
            placement = position;
            size = currentSize;
        }

        public ItemContent(Item inputItem,GameObject itemReference, int position, int currentSize)//Construnctor for new Items
        {
            item = inputItem;
            reference = itemReference;
            placement = position;
            size = currentSize;
        }

    }

    //Emum for the different types of inventories
    public enum InventoryType { Player,Shopkeeper,Storage }

    public int rowCapacity;

    public List<ItemContent> itemList;//List of all the Items in the Inventory


    public Transform InventorySlots;//Variable for the main container that holds all other smaller containers
    public static GameObject ItemContainer { get { if(ItemContainerPrefab == null) ItemContainerPrefab = Resources.Load("Prefabs/Inventory/ItemContainer") as GameObject; return ItemContainerPrefab; }  } //Prefab for Item containers
    public static GameObject ItemContainerPrefab;

    //Method that spawn Item Containers forthe Player Inventory
    public static void SpawnItemContainers(int divisionNumber,InventoryType inventoryType, Transform InventoryContainer, List<ItemContent> list)
    {
        int itemNumber;
        switch (inventoryType)
        {
            case InventoryType.Player://The Player Inventory holds 16 items on rows with 8 items
                itemNumber = 16;
                break;
            case InventoryType.Shopkeeper://The Shopkeepe Inventory holds 24 items on rows with 6 items
                itemNumber = 24;
                break;
            case InventoryType.Storage://The Storage Inventory holds 40 items items on rows with 8 items
                itemNumber = 40;
                break;
            default:
                itemNumber = 0;
                break;
        }

        for (int i = 0; i < itemNumber; ++i)
        {
            GameObject itemContainer = Instantiate(ItemContainer, InventoryContainer);//Spawn Containers

            itemContainer.GetComponent<Container>().itemList = list;

            itemContainer.transform.localPosition = new Vector3(120f * (i%divisionNumber), -130f*(int)(i/divisionNumber), 0f);//Containers have a custom position in the Canvas UI

            itemContainer.transform.localPosition -= new Vector3(50f, 50f, 0f); //Accounts for the Item container Pivot
        }
    }

    //Method that spawns all inventory items
    public static void SpawnContents(InventoryType inventoryType,int divisionNumber, Transform InventoryContainer,List<ItemContent> list)
    {
        if (!GetContainersSpawned(InventoryContainer))
        SpawnItemContainers(divisionNumber, inventoryType, InventoryContainer,list);

        foreach (ItemContent item in list)
            GameController.CreateItem(item,list,InventoryContainer);
    }

    public void Update()
    {
        InventoryBehaviour();
    }

    private static bool GetContainersSpawned(Transform container)
    {
        if (container.childCount == 0) return false;
        return true;
    }

    public static void ClearContents(Transform InventoryContainer)
    {
        Item[] items = InventoryContainer.GetComponentsInChildren<Item>();

        foreach (Item i in items)
        {
            Destroy(i.gameObject);
        }

    }

    //Abstract Methods that are overrided in inventory type scripts
    abstract protected void InventoryBehaviour();

}
