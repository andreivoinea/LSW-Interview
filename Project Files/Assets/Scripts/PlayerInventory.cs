using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : InventoryManager
{
    //This Script handles all player inventory methods and variables
    //
    //

    //Inventory camera for real-time equipment updates
    [HideInInspector] public CameraController inventoryCamera;

    //UI that corresponds to player's gold
    public TextMeshProUGUI currencyText;

    public Transform EquipmentContainer;

    private new void Update()
    {
        base.Update();
        currencyText.text = "Gold: " + GameController.Instance.Player.Currency;//Updates the Currency UI
    }

    //Method that gets the Player Inventory Camera
    public void SetInventoryCamera()
    {
        inventoryCamera = GetComponentInChildren<CameraController>();
    }

    //Method that handles the Inventory Camera
    protected override void InventoryBehaviour()
    {
        if (inventoryCamera == null) SetInventoryCamera();//Load the Inventory Camera

       inventoryCamera.SetTarget(GameController.Instance.Player.gameObject);//set's the inventory camera to follow the player
    }

    public List<ItemContent> GetEquipment()
    {
        List<ItemContent> itemContentList = new List<ItemContent>();
        List<Item> itemList = new List<Item>(EquipmentContainer.GetComponentsInChildren<Item>());

        foreach (Item item in itemList)
        {
            itemContentList.Add(item.content);
        }

        return itemContentList;
    }

    public void SetEquipment(List<ItemContent> itemList) 
    {
        foreach (ItemContent item in itemList)
        {
                    GameController.CreateItem(item, EquipmentContainer.GetChild(item.placement).GetComponent<Container>().itemList, EquipmentContainer, true);
                    GameController.Instance.Player.EquipItem((int)item.item.itemType, item.item.inGameTexture);
        }
    }

}
