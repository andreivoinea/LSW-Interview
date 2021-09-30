using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameController))]
public class StorageManager : MonoBehaviour
{

    [SerializeField] public List<Item> allItems;

    private static bool loaded = false;

    [System.Serializable]
    private class itemWrapper
    {
        public InventoryManager.ItemContent[] array;

        public itemWrapper()
        {
            array = new InventoryManager.ItemContent[0];
        }
        public itemWrapper(List<InventoryManager.ItemContent> list) {
            array = new InventoryManager.ItemContent[list.Count];
            for (int i = 0; i < list.Count; ++i) array[i] = list[i];
        }
    }

    private void Start()
    {
        GetPrefabList();
    }

    public static void SaveInvetory(InventoryManager im)
    {
        itemWrapper w = new itemWrapper(im.prefabContents);
        string save = JsonUtility.ToJson(w);
        PlayerPrefs.SetString("PlayerInventory", save);
    }

    public static void LoadInvetory(InventoryManager im)
    {
        if (loaded) return;
        string jsonContents;
        jsonContents = PlayerPrefs.GetString("PlayerInventory", "");

        itemWrapper w = new itemWrapper();

        if (jsonContents != "")
            JsonUtility.FromJsonOverwrite(jsonContents, w);

        im.prefabContents = new List<InventoryManager.ItemContent>(w.array);
    }

    public static void SavePlayerStats(PlayerController player)
    {
        PlayerPrefs.SetInt("PlayerCurrency", player.Currency);
    }

    public static void LoadPlayerStats(PlayerController player)
    {
        if (loaded) return;
        player.Currency = PlayerPrefs.GetInt("PlayerCurrency", 0);
    }

    public void GetPrefabList()
    {
        Object[] list = Resources.LoadAll("Prefabs", typeof(Item));
        allItems = new List<Item>();

        foreach (Object o in list)
        {
            allItems.Add(o as Item);
        }
    }

    public static void Save() 
    {
        SavePlayerStats(GameController.Instance.Player);
        SaveInvetory(GameController.Instance.Inventory);
    }
    public static void Load()
    {
        LoadPlayerStats(GameController.Instance.Player);
        LoadInvetory(GameController.Instance.Inventory);

        loaded = true;
    }
    

}
