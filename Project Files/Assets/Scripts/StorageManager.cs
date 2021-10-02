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

    //Method that saves an Inventory
    public static void SaveInventory(InventoryManager im, string saveString)
    {
        SaveInventory(im.itemList,saveString);
    }
    public static void SaveInventory(List<InventoryManager.ItemContent> itemList, string saveString)
    {
        itemWrapper w = new itemWrapper(itemList);//Creates a new wrapper from the item list
        string save = JsonUtility.ToJson(w);//Formats the Item data to JSON
        PlayerPrefs.SetString(saveString, save);//Saves the JSON to memory

    }

        //Method that loads Player Inventory to the Inventory Manager
        public static void LoadInventory(InventoryManager im,string searchString)
    {
        im.itemList = LoadInventory(searchString);
    }

    public static List<InventoryManager.ItemContent> LoadInventory(string searchString) 
    {
        string jsonContents = PlayerPrefs.GetString(searchString, "");//Loads the JSON from memory

        itemWrapper w = new itemWrapper();//Creates a new wrapper to translate the JSON into data

        if (jsonContents != "")
            JsonUtility.FromJsonOverwrite(jsonContents, w);//Formats the JSON to Item data

        return new List<InventoryManager.ItemContent>(w.array);//Fills the prefab list
    }

    //Method that saves Player Stats
    public static void SavePlayerStats(PlayerController player)
    {
        PlayerPrefs.SetInt("PlayerCurrency", player.Currency);//Saves the player's currency
    }

    //Method that loads Player Stats
    public static void LoadPlayerStats(PlayerController player)
    {
        int gold = PlayerPrefs.GetInt("PlayerCurrency", -1);
        if (gold == -1) player.Currency = 2000; //Loads the player with the starting ammount of currency
        else
            player.Currency = gold;//Loads the player's currency
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
        SaveInventory(GameController.Instance.Inventory, "PlayerInventory");//Saves Player Inventory
        SaveInventory(GameController.Instance.Inventory.GetEquipment(),"PlayerEquipment");//Saves Player Equipment
    }

    //Method that loads all the player information
    public static void Load()
    {
        if (loaded) return;

        LoadPlayerStats(GameController.Instance.Player);//Loads Player Stats
        LoadInventory(GameController.Instance.Inventory, "PlayerInventory");//Loads Player Inventory
        GameController.Instance.Inventory.SetEquipment(LoadInventory("PlayerEquipment"));

        loaded = true;//Remembers that data was already loaded
    }
    

}
