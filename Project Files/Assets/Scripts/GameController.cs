using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    private void Awake()
    {

        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else _instance = this;

        StorageManager.Load();
    }

    public InventoryManager Inventory;
    public PlayerController Player;

    public void ShowInventory(bool open)
    {
        if (open == Inventory.gameObject.activeSelf) return;

        Inventory.gameObject.SetActive(open);
        if (open) Inventory.SpawnContents();

    }

    public void SetPlayer(PlayerController player)
    {
        Player = player;
    }

    private void Update()
    {
        InventoryBehaviour();
    }

    private void InventoryBehaviour()
    {
        if (Inventory == null) return;
        if (Inventory.inventoryCamera == null) return;

        Inventory.inventoryCamera.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Inventory.inventoryCamera.transform.position.z);
    }

    public Item GetItem(Item mainItem)
    {

        foreach (Item item in GetComponent<StorageManager>().allItems)
        {
            if (item.Name == mainItem.Name) return item;
        }

        return null;
    }

    private void OnApplicationQuit()
    {
        StorageManager.Save();
    }

    public bool CheckCurrency(int price)
    {
        if (price > Player.Currency) return false;
        return true;
    }
    public void AddCurrency(int gold)
    {
        Player.Currency += gold;
    }

    public static void CreateItem(InventoryManager.ItemContent item, bool addToList = false)
    {
        item.reference = Instantiate(Instance.GetItem(item.item).gameObject, Instance.Inventory.InventorySlots.transform.GetChild(item.placement), false);
        item.reference.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = item.size.ToString();
        if (addToList) Instance.Inventory.AddItem(item);
    }

}
