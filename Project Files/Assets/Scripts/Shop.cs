using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : NPCController
{
    //This Script is attached to a specific type of NPCs, mainly Shopkeepers. It Contains Methods for the Shopkeeper's inventory as well as it's UI
    //
    //

    //Variable to access the Shopkeeper's Inventory
    public ShopkeeperInventory Inventory;

    //Variable to access the Shopkeeper
    public GameObject Shopkeeper;

    //Variables that correspond to the Trading System
    public Transform TradeContainer;
    public Toggle BuyButton;
    public Toggle SellButton;
    public Button ConfirmButton;

    private enum ShopMode { Buy,Sell }
    private ShopMode currentShopMode;

    public List<InventoryManager.ItemContent> playerItemList;//List of all the Items in the Player Inventory

    public GameObject InteractUI;

    //Inherited method that shows the interact UI
    override protected void ShowUI(bool interactStatus)
    {
        InteractUI.SetActive(!interactStatus);
    }

    override protected void OnInteracted(bool interacted,out bool returnStatus)
    {
        base.OnInteracted(interacted,out returnStatus);
        if (returnStatus) return;

        ShowInventory(interacted);
    }

    private void ShowInventory(bool open)
    {
        Inventory.gameObject.SetActive(open);

        if (open)
            BuyPanel();
        else
        {
            InventoryManager.ClearContents(TradeContainer);
            InventoryManager.ClearContents(Inventory.InventorySlots);
            InventoryManager.ClearContents(Inventory.PlayerInventoryHolder);
        }
    }

    //Method that starts the Buy functionality of the Shop
    public void BuyPanel()
    {
        BuyButton.interactable = false;
        SellButton.interactable = true;

        Inventory.PlayerInventoryHolder.gameObject.SetActive(false);
        Inventory.InventorySlots.gameObject.SetActive(true);

        InventoryManager.SpawnContents(InventoryManager.InventoryType.Shopkeeper, Inventory.rowCapacity, Inventory.InventorySlots,Inventory.itemList);
        InventoryManager.ClearContents(Inventory.PlayerInventoryHolder);

        currentShopMode = ShopMode.Buy;
    }

    //Method that starts the Sell functionality of the Shop
    public void SellPanel()
    {
        BuyButton.interactable = true;
        SellButton.interactable = false;

        playerItemList = GameController.Instance.Inventory.itemList;

        Inventory.InventorySlots.gameObject.SetActive(false);
        Inventory.PlayerInventoryHolder.gameObject.SetActive(true);

        InventoryManager.SpawnContents(InventoryManager.InventoryType.Player, Inventory.rowCapacity, Inventory.PlayerInventoryHolder,playerItemList);
        InventoryManager.ClearContents(Inventory.InventorySlots);

        currentShopMode = ShopMode.Sell;
    }

    public void ConfirmTransaction()
    {
        TradeInfo trade = new TradeInfo(this);
        if (trade.spaceNeeded == 0) return; //insert items first

        switch (currentShopMode) {
            case ShopMode.Buy:             
               
                if(!GameController.Instance.CheckCurrency(trade.value)) return; //not enough money

                if (!GameController.Instance.CheckInventoryEmptySpaces(trade.spaceNeeded)) return; // no place in inventory

                InsertTradeItems(trade.spaceNeeded);

                GameController.Instance.ModifyCurrency(-trade.value);

                break;
            case ShopMode.Sell:

                GameController.Instance.ModifyCurrency(trade.value);

                break;
        }

        InventoryManager.ClearContents(TradeContainer);
    }

    public class TradeInfo 
    {
        public int value;
        public int spaceNeeded;

        public TradeInfo(Shop shop)
        {
            shop.CalculateTradeInfo(out value, out spaceNeeded);
        }
    }

    private void CalculateTradeInfo(out int value, out int spaceNeeded)
    {
        value = 0;
        spaceNeeded = 0;
        foreach (Container c in TradeContainer.GetComponentsInChildren<Container>())
        {
            if (c.itemList.Count == 0) continue;

            int size;
            if (!c.itemList[0].item.isStackable) size = 1;
            else size = c.itemList[0].Size;

            value += size * c.itemList[0].item.price;
            ++spaceNeeded;
        }
    }

    private void InsertTradeItems(int spaceNeeded)
    {
        List<int> emptySpaces = GameController.Instance.GetInventoryEmptySpaces(spaceNeeded);

        foreach (Container c in TradeContainer.GetComponentsInChildren<Container>())
        {
            if (c.itemList.Count == 0) continue;

            InventoryManager.ItemContent item = c.itemList[0];

            item.placement = emptySpaces[spaceNeeded - 1];
            --spaceNeeded;

            GameController.Instance.Inventory.itemList.Add(item);
        }
    }

}
