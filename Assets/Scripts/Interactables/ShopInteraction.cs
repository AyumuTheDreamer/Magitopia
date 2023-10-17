using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopInteraction : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject inventoryPanel;
    public InventoryManager inventoryManager;
    private PlayerMovement playerMovement;
    public List<Item> shopInventory = new List<Item>();
    public GameObject ShopItemPrefab;
    public Transform shopItemContent;
    public bool isShopOpen { get; private set; } = false;
    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found in the scene!");
        }
        
    }

    public void Interact()
    {
        Debug.Log("Interact method called on " + gameObject.name);

        shopPanel.SetActive(!shopPanel.activeSelf);
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        playerMovement.isInventoryOpen = inventoryPanel.activeSelf;

        isShopOpen = shopPanel.activeSelf;  // Update the state of the ShopUI

        if (inventoryPanel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        UpdateShopUI();
        inventoryManager.ListItems();
    }




public void SellShopItem(Item item)
{
    Debug.Log("AddItemToShop called with item: " + item.itemName);
    if (shopInventory == null || inventoryManager == null)
    {
        Debug.LogError("shopInventory or inventoryManager is null. Make sure they are properly assigned.");
        return;
    }

    // Transfer the item from the player's inventory to the shop's inventory.
    inventoryManager.Remove(item); // Remove from player's inventory.
    shopInventory.Add(item); // Add to the shop's inventory.

    inventoryManager.ListItems();
    UpdateShopUI();
}

public void UpdateShopUI()
{
    Debug.Log("UpdateShopUI called");

    Transform shopItemContent = shopPanel.transform.Find("ShopPanel/ShopItemContent");

    if (shopItemContent == null)
    {
        Debug.LogError("shopItemContent not found in the hierarchy!");
        return;
    }

    foreach (var item in shopInventory)
    {
        Debug.Log("Item ID: " + item.id);
        Debug.Log("Item Icon: " + item.itemIcon);

        Debug.Log("Item found in shopInventory: " + item.itemName);
        GameObject shopItemObject = Instantiate(ShopItemPrefab, shopItemContent);
        var itemName = shopItemObject.transform.Find("ItemName").GetComponent<Text>();
        var itemIcon = shopItemObject.transform.Find("ItemIcon").GetComponent<Image>();
        var sellButton = shopItemObject.transform.Find("SellButton").GetComponent<Button>();

        itemName.text = item.itemName;
        itemIcon.sprite = item.itemIcon;

        // Add a click event to the sell button.
        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(() => SellShopItem(item));
        if (item.isStackable)
            {
                // Check if it's a stackable item and set the item count text.
                var itemCountText = shopItemObject.transform.Find("ItemCount").GetComponent<Text>();
                itemCountText.text = "x" + item.quantity.ToString();
            }
            else
            {
                // If it's not stackable, hide the item count text (if it exists).
                var itemCountText = shopItemObject.transform.Find("ItemCount").GetComponent<Text>();
                if (itemCountText != null)
                {
                    itemCountText.text = "";
                }
            }
    }
}
}
