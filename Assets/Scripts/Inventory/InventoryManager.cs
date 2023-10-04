using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();
    

    public Transform ItemContent;
    public GameObject InventoryItem;

    public Toggle EnableRemove;

    private bool isInventoryOpen = false;

    private void Awake()
    {
        Instance = this;

    }

    private void Update()
    {
        if (isInventoryOpen)
        {
            ListItems();
        }
    }

    public void Add(Item item)
{
    if (item.isStackable)
    {
        // Check if there's an existing stack of this item in the inventory.
        foreach (var existingItem in Items)
        {
            if (existingItem.id == item.id && existingItem.quantity < existingItem.maxStackCount)
            {
                // Increment the quantity of the existing stack.
                existingItem.quantity++;
                return; // Exit the method after stacking.
            }
        }
    }

    // If the item is not stackable or no stack was found, add it as a new item.
    Items.Add(item);
}


    public void Remove(Item item)
    {
        Items.Remove(item);
    }

    public void ListItems()
{
    foreach (Transform item in ItemContent)
    {
        Destroy(item.gameObject);
    }

    foreach (var item in Items)
    {
        GameObject obj = Instantiate(InventoryItem, ItemContent);
        var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
        var removeButton = obj.transform.Find("RemoveButton").GetComponent<Button>();

        itemName.text = item.itemName;

        if (item.isStackable)
        {
            // Check if it's a stackable item and set the item count text.
            var itemCountText = obj.transform.Find("ItemCount").GetComponent<Text>();
            itemCountText.text = "x" + item.quantity.ToString();
        }
        else
        {
            // If it's not stackable, hide the item count text (if it exists).
            var itemCountText = obj.transform.Find("ItemCount").GetComponent<Text>();
            if (itemCountText != null)
            {
                itemCountText.text = "";
            }
        }

        if (EnableRemove.isOn)
            removeButton.gameObject.SetActive(true);
    }
}


    public void EnableItemsRemove()
{
    if (EnableRemove.isOn)
    {
        Debug.Log("EnableRemove is ON");
        foreach (Transform item in ItemContent)
        {
            Transform removeButton = item.Find("RemoveButton");
            if (removeButton != null)
            {
                Debug.Log("Setting RemoveButton active");
                removeButton.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("RemoveButton not found in item.");
            }
        }
    }
    else
    {
        Debug.Log("EnableRemove is OFF");
        foreach (Transform item in ItemContent)
        {
            Transform removeButton = item.Find("RemoveButton");
            if (removeButton != null)
            {
                Debug.Log("Setting RemoveButton inactive");
                removeButton.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("RemoveButton not found in item.");
            }
        }
    }
}
    public void ToggleInventory(bool isOpen)
    {
        isInventoryOpen = isOpen;
    }


}
