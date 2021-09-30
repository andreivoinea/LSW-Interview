using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : NPCController
{
    //This Script is attached to a specific type of NPCs, mainly Shopkeepers. It Contains Methods for the Shopkeeper's inventory as well as it's UI
    //
    //

    //Variable to access the Shopkeeper's Inventory
    public ShopkeeperInventory Inventory;

    //Variable to access the Shopkeeper
    public GameObject Shopkeeper;

    //Inherited method that shows the interact UI
    override protected void ShowUI() 
    {
        Debug.Log("ShowUI");
    }

    override protected void OnInteracted(bool interacted)
    {
        Debug.Log("show inventory: " + interacted);
        ShowInventory();

    }

    public bool spawnedContents = false;

    private void ShowInventory()
    {
        if (spawnedContents) return;
        Inventory.gameObject.SetActive(true);
        Inventory.SpawnContents(InventoryManager.InventoryType.Shopkeeper, Inventory);
        spawnedContents = true;
    }

}
