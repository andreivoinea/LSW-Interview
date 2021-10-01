using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //This Script is attached to the GameController. The Game Controller is a central point to controlling most functionability of the game.
    //It Contains methods for Player Stats, Player Inventory, Item Management and Data Management.
    //

    //Variables that hold the GameController singleton instance in memory
    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    private void Awake()
    {
        //The Game Controller is a singleton, meaning only one can exist at a time in a Scene, the following lines enshure that there is only one GameController in a Scene, and that it can be called from any other Script using the Instance variable
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else _instance = this;
    }

    private void Start()
    {
        StorageManager.Load();//Load Player's Inventory from Storage
    }

    //Variable to access Player's Inventory
    public PlayerInventory Inventory;

    //Variable to access the Player
    public PlayerController Player;

    //Method that sets the reference to the player
    public void SetPlayer(PlayerController player)
    {
        Player = player;
    }

    //Variable that hold information about the Player's action to open or close the Inventory Tab
    public bool inventoryStatus = false;

    //Method that shows the Inventory Panel UI
    public void ShowInventory(bool setStatus)
    {
        if (!GameController.Instance.IsPlayerInteracting()) inventoryStatus = setStatus;

        if (inventoryStatus == Inventory.gameObject.activeSelf) return; // makes sure the player dosen't try to open the inventory while it is already open or close it when it's closed

        //Opens Or Closes the Inventory Panel
        Inventory.gameObject.SetActive(inventoryStatus);

        //Spawns Player Items in case that the inventory is opened
        if (inventoryStatus) InventoryManager.SpawnContents(InventoryManager.InventoryType.Player, Inventory.rowCapacity, Inventory.InventorySlots, Inventory.itemList);
        else InventoryManager.ClearContents(Inventory.InventorySlots);
    }

    public bool CheckInventoryEmptySpaces(int neededSpaces)
    {
        if (neededSpaces < 16 - Inventory.itemList.Count) return true;
        return false;

    }

    public List<int> GetInventoryEmptySpaces(int neededSpaces)
    {
        List<int> emptySpaces = new List<int>();

        int[] aux = new int[16];

        foreach (InventoryManager.ItemContent item in Inventory.itemList)
        {
            ++aux[item.placement];
        }

        for (int i = 0; i < 16; ++i)
            if (aux[i] == 0)
            {
                emptySpaces.Add(i);
                if (emptySpaces.Count == neededSpaces) return emptySpaces;
            }


        return emptySpaces;

    }
    
    //Method that check if the player has enough currency to purchase an item
    public bool CheckCurrency(int price)
    {
        if (price > Player.Currency) return false;
        return true;
    }

    //Method that adds or subtracts from the player's currency. if(CheckCurrency(price)) should always be called before performing a purchase.
    public void ModifyCurrency(int gold)
    {
        Player.Currency += gold;
    }

    //Method that returns the item prefab referebce from the prefab list. Used to spawn a new item
    public Item GetItem(Item mainItem)
    {
        if (mainItem == null) return null;
        foreach (Item item in GetComponent<StorageManager>().allItems)//searches the item in all the item list
        {
            if (item.Name == mainItem.Name) return item;
        }

        return null;
    }

    //Method that handles Items Spawning in their Specified Inventory Slots
    public static void CreateItem(InventoryManager.ItemContent item, List<InventoryManager.ItemContent> list,Transform inventoryContainer, bool addToList = false)//Specifies the item to create and if it should be added to the item list .The initial list is created once it loads form memory, and any subsequent item creation should have the addToList flag set to true
    {
        if (item == null) return;
        if (item.item == null) return;

        item.reference = Instantiate(Instance.GetItem(item.item).gameObject, inventoryContainer.GetChild(item.placement), false);//Creates the item at the specified position
        item.reference.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = item.size.ToString();//Loads the items UI to match the current number of items 
        if (addToList) list.Add(item);//Adds the item to the item list
    }

    private bool playerInteracting;
    public void PlayerInteracting(bool status)
    {
        playerInteracting = status;
    }
    public bool IsPlayerInteracting() {
        return playerInteracting;
    }

    //Ensures that every time the Player closes the game, it's Inventory is saved;
    private void OnApplicationQuit()
    {
        StorageManager.Save();//Save the Player inventory to memory
    }

}
