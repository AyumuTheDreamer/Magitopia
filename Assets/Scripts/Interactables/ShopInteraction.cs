using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopInteraction : MonoBehaviour
{
    public GameObject potionShopPanel;
    public GameObject inventoryPanel;
    public InventoryManager inventoryManager;
    private PlayerMovement playerMovement;
    public List<Item> shopInventory = new List<Item>();
    public GameObject ShopItemPrefab;
    public Transform shopItemContent;
    public ThirdPersonCam thirdPersonCam;
    public bool isShopOpen { get; private set; } = false;
    public float minSellInterval = 5f; // Minimum time interval to sell an item
    public float maxSellInterval = 15f;
    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found in the scene!");
        }
        StartCoroutine(AutoSellItemsCoroutine());
    }

    public void Interact()
    {
        Debug.Log("Interact method called on " + gameObject.name);

        potionShopPanel.SetActive(!potionShopPanel.activeSelf);
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        playerMovement.isInventoryOpen = inventoryPanel.activeSelf;

        isShopOpen = potionShopPanel.activeSelf;  // Update the state of the ShopUI

        if (inventoryPanel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            thirdPersonCam.LockCameraOrientation();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            thirdPersonCam.UnlockCameraOrientation();
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

    // Try to find the item in the shop's inventory
    Item shopItem = shopInventory.Find(shopItem => shopItem.id == item.id);

    if (shopItem != null)
    {
        // Item is already in the shop's inventory, update its quantity.
        shopItem.quantity += item.quantity;
    }
    else
    {
        // Item is not in the shop's inventory, add it as a new item.
        shopInventory.Add(item);
        
    }
    if (item.itemName == "Attack Potion")
        {
            // Replace "sellAttackPotion" with the actual ID for this objective
            ObjectiveManager.Instance.CompleteObjective("sellAttackPotion");
        }
    // Remove the item from the player's inventory.
    inventoryManager.Remove(item);

    // Update the shop's UI.
    UpdateShopUI();

    // Update the player's inventory UI.
    inventoryManager.ListItems();
}




public void UpdateShopUI()
{
    Debug.Log("UpdateShopUI called");

    Transform shopItemContent = potionShopPanel.transform.Find("ShopPanel/ShopItemContent");

    if (shopItemContent == null)
    {
        Debug.LogError("shopItemContent not found in the hierarchy!");
        return;
    }

    // Clear the existing shop item content.
    foreach (Transform item in shopItemContent)
    {
        Destroy(item.gameObject);
    }

    // Add items from the shop's inventory list.
    foreach (var item in shopInventory)
    {
        GameObject shopItemObject = Instantiate(ShopItemPrefab, shopItemContent);
        var itemName = shopItemObject.transform.Find("ItemName").GetComponent<Text>();
        var itemIcon = shopItemObject.transform.Find("ItemIcon").GetComponent<Image>();
        var sellButton = shopItemObject.transform.Find("SellButton").GetComponent<Button>();

        // Debug statement to check item names
        Debug.Log($"Checking item for sell button visibility: {item.itemName}");

        // Set the sell button active state based on the item name containing "Potion"
        bool hasPotionInName = item.itemName.Contains("Potion");
        sellButton.gameObject.SetActive(hasPotionInName);

        // Set up sell button if it's a potion
        if (hasPotionInName)
        {
            sellButton.onClick.RemoveAllListeners(); // Remove previous listeners to avoid stacking listeners.
            sellButton.onClick.AddListener(() => SellShopItem(item));
        }

        itemName.text = item.itemName;
        itemIcon.sprite = item.itemIcon;

        // Check if it's a stackable item and set the item count text.
        if (item.isStackable)
        {
            var itemCountText = shopItemObject.transform.Find("ItemCount").GetComponent<Text>();
            itemCountText.text = $"x{item.quantity}";
        }
        else
        {
            // If it's not stackable, hide the item count text.
            var itemCountText = shopItemObject.transform.Find("ItemCount").GetComponent<Text>();
            if (itemCountText != null)
            {
                itemCountText.gameObject.SetActive(false);
            }
        }
    }
}
 private IEnumerator AutoSellItemsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSellInterval, maxSellInterval));

            if (shopInventory.Count > 0)
            {
                SellRandomShopItem();
            }
        }
    }


     private void SellRandomShopItem()
{
    int randomIndex = Random.Range(0, shopInventory.Count);
    Item itemToSell = shopInventory[randomIndex];

    // Add the value of the sold item to the player's currency
    CurrencyManager.Instance.AddCurrency(itemToSell.value * itemToSell.quantity);

    // Check if there are more than one of the item and reduce the quantity or remove
    if (itemToSell.quantity > 1)
    {
        itemToSell.quantity--;
    }
    else
    {
        shopInventory.Remove(itemToSell);
    }

    // Update the shop's UI and possibly other elements such as the player's currency
    UpdateShopUI();
}


}
