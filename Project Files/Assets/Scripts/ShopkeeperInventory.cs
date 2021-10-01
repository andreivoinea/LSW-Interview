using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ShopkeeperInventory : InventoryManager
{
    //This Script handles all shopkeeper inventory methods and variables
    //
    //

    //Shopkeeper camera for real-time animations
    [HideInInspector] public CameraController shopkeeperCamera;

    //UI that corresponds to player's gold
    public TextMeshProUGUI playerCurrencyText;

    //Variable for the player invetory container that holds all other smaller containers. Used to sell items from the player's inventory.
    public Transform PlayerInventoryHolder;

    private new void Update()
    {
        base.Update();
        playerCurrencyText.text = "Gold: " + GameController.Instance.Player.Currency;//Updates the Currency UI
    }

    //Method that gets the Shopkeeper Inventory Camera
    public void SetInventoryCamera()
    {
        shopkeeperCamera = GetComponentInChildren<CameraController>();
    }

    //Method that handles the Inventory Camera
    protected override void InventoryBehaviour()
    {
        if (shopkeeperCamera == null) SetInventoryCamera();//Load the Inventory Camera

        shopkeeperCamera.SetTarget(GetComponentInParent<Shop>().Shopkeeper.gameObject);//set's the inventory camera to follow the player
    }



}
