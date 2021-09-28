using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [HideInInspector]
    public Camera inventoryCamera;

    private List<Item> contents = new List<Item>();

    public Transform InventorySlots;
    public GameObject ItemContainer;
   

    private void Start()
    {
        inventoryCamera = GetComponentInChildren<Camera>();

        SpawnItemContainers();
    }

    private void SpawnItemContainers()
    {
        for (int i = 0; i < 16; ++i)
        {
            GameObject itemContainer = Instantiate(ItemContainer, InventorySlots);

            itemContainer.transform.localPosition = new Vector3(12f * (i%8), i<8?0f:-24f, 0f);
        }

    }
}
