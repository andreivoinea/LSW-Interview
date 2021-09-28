using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public InventoryManager Inventory;

    private PlayerController Player;

    public void ShowInventory(bool open)
    {
        Inventory.gameObject.SetActive(open);
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
        if(Inventory==null) return;
        if(Inventory.inventoryCamera == null) return;

        Inventory.inventoryCamera.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Inventory.inventoryCamera.transform.position.z);
    }
}
