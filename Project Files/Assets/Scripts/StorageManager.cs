using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameController))]
public class StorageManager : MonoBehaviour
{
    //This Script is attached to the GameController. It handles Data storage and retreival
    //
    //

    //List of all the Prefab items
    [SerializeField] public List<Item> allItems;

    //Variable that determines if the list is already loaded
    private static bool loaded = false;

    //Item Wrapper for transforming the inventory data into a serializable object to format Items into JSON
    [System.Serializable]
    private class itemWrapper
    {
        public InventoryManager.ItemContent[] array;

        //constructor for an empty list
        public itemWrapper()
        {
            array = new InventoryManager.ItemContent[0];
        }

        //constructor to generate a the array form a list
        public itemWrapper(List<InventoryManager.ItemContent> list) {
            array = new InventoryManager.ItemContent[list.Count];
            for (int i = 0; i < list.Count; ++i) array[i] = list[i];
        }
    }

    private void Awake()
    {
        GetPrefabList();
    }

    //Method that saves Player Inventory from the Inventory Manager
    public static void SaveInvetory(InventoryManager im)
    {
        itemWrapper w = new itemWrapper(im.itemList);//Creates a new wrapper from the item list
        string save = JsonUtility.ToJson(w);//Formats the Item data to JSON
        PlayerPrefs.SetString("PlayerInventory", save);//Saves the JSON to memory
    }

    //Method that loads Player Inventory to the Inventory Manager
    public static void LoadInvetory(InventoryManager im)
    {
        if (loaded) return;
        string jsonContents = PlayerPrefs.GetString("PlayerInventory", "");//Loads the JSON from memory

        itemWrapper w = new itemWrapper();//Creates a new wrapper to translate the JSON into data

        if (jsonContents != "")
            JsonUtility.FromJsonOverwrite(jsonContents, w);//Formats the JSON to Item data

        im.itemList = new List<InventoryManager.ItemContent>(w.array);//Fills the prefab list
    }

    //Method that saves Player Stats
    public static void SavePlayerStats(PlayerController player)
    {
        PlayerPrefs.SetInt("PlayerCurrency", player.Currency);//Saves the player's currency
    }

    //Method that loads Player Stats
    public static void LoadPlayerStats(PlayerController player)
    {
        if (loaded) return;
        player.Currency = PlayerPrefs.GetInt("PlayerCurrency", 0);//Loads the player's currency
    }

    //Method that loads all the item Prefab to the prefab list
    public void GetPrefabList()
    {
        Object[] list = Resources.LoadAll("Prefabs", typeof(Item));//Searhes for all the Item Prefabs
        allItems = new List<Item>();

        foreach (Object o in list)
        {
            allItems.Add(o as Item);
        }
    }

    //Method that saves all the player information
    public static void Save() 
    {
        SavePlayerStats(GameController.Instance.Player);//Saves Player Stats
        SaveInvetory(GameController.Instance.Inventory);//Saves Player Inventory
    }

    //Method that loads all the player information
    public static void Load()
    {
        LoadPlayerStats(GameController.Instance.Player);//Loads Player Stats
        LoadInvetory(GameController.Instance.Inventory);//Loads Player Inventory

        loaded = true;//Remembers that data was already loaded
    }
    

}
