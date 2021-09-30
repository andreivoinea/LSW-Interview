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
            item = inputItem;;
            placement = position;
            size = currentSize;
        }
    }

    //Emum for the different types of inventories
    public enum InventoryType { Player,Shopkeeper,Storage }

    public List<ItemContent> itemList;//List of all the Items in the Inventory


    public Transform InventorySlots;//Variable for the main container that hold all other smaller containers
    public GameObject ItemContainer;//Prefab for Item containers

    //Variable that remembers if the containers already spawned
    private bool containersSpawned;

    //Method that spawn Item Containers forthe Player Inventory
    public void SpawnItemContainers(InventoryType inventoryType)
    {
        int itemNumber;
        int divisionNumber;
        switch (inventoryType)
        {
            case InventoryType.Player://The Player Inventory holds 16 items on rows with 8 items
                itemNumber = 16;
                divisionNumber = 8;
                break;
            case InventoryType.Shopkeeper://The Shopkeepe Inventory holds 24 items on rows with 6 items
                itemNumber = 24;
                divisionNumber = 6;
                break;
            case InventoryType.Storage://The Storage Inventory holds 40 items items on rows with 8 items
                itemNumber = 40;
                divisionNumber = 5;
                break;
            default:
                itemNumber = 0;
                divisionNumber = 0;
                break;
        }

        for (int i = 0; i < itemNumber; ++i)
        {
            GameObject itemContainer = Instantiate(ItemContainer, InventorySlots);//Spawn Containers

            itemContainer.transform.localPosition = new Vector3(120f * (i%divisionNumber), -130f*(int)(i/divisionNumber), 0f);//Containers have a custom position in the Canvas UI

            itemContainer.transform.localPosition -= new Vector3(50f, 50f, 0f); //Accounts for the Item container Pivot
        }

        containersSpawned = true;
    }

    //Method that spawns all inventory items
    public void SpawnContents(InventoryType inventoryType,InventoryManager inventory)
    {
        if (containersSpawned) return;
        SpawnItemContainers(inventoryType);

        foreach (ItemContent item in itemList)
            GameController.CreateItem(item,inventory);
    }

    //Adds new item to the inventory list
    public void AddItem(ItemContent item)
    {
        itemList.Add(item);
    }

    public void Update()
    {
        InventoryBehaviour();
    }

    //Abstract Methods that are overrided in inventory type scripts
    abstract protected void InventoryBehaviour();

}
