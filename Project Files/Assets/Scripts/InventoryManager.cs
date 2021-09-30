using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [HideInInspector]public Camera inventoryCamera;
    public TextMeshProUGUI currencyText;

    [System.Serializable]
    public class ItemContent {
        [SerializeField] public Item item;
        [SerializeField] public GameObject reference;
        public int placement;
        public int size;
        public int Size { get { return size; } set { size = value; reference.GetComponent<Item>().CurrentItemStack = value; } }

        public ItemContent(Item inputItem, int position, int currentSize)
        {
            item = inputItem;;
            placement = position;
            size = currentSize;
        }
    }
//[HideInInspector]
    public List<ItemContent> prefabContents;


    public Transform InventorySlots;
    public GameObject ItemContainer;

    private bool containersSpawned;
    public void SpawnPlayerItemContainers()
    {
        for (int i = 0; i < 16; ++i)
        {
            GameObject itemContainer = Instantiate(ItemContainer, InventorySlots);

            itemContainer.transform.localPosition = new Vector3(120f * (i%8)-50f, i<8?0f:-130f, 0f);
        }

        containersSpawned = true;
    }

    public void SpawnShopItemContainers()
    {
        for (int i = 0; i < 32; ++i)
        {
            GameObject itemContainer = Instantiate(ItemContainer, InventorySlots);

            itemContainer.transform.localPosition = new Vector3(12f * (i % 8), i < 8 ? 0f : -24f, 0f);
        }

        containersSpawned = true;
    }

    public void SpawnContents()
    {
        if (containersSpawned) return;
        SpawnPlayerItemContainers();

        foreach (ItemContent item in prefabContents)
            GameController.CreateItem(item);
    }

    public void SetPlayerInventoryCamera()
    {
        inventoryCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        currencyText.text = "Gold: " + GameController.Instance.Player.Currency;
    }

    public void AddItem(ItemContent item)
    {
        prefabContents.Add(item);
    }


}
