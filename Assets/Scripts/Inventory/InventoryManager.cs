using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;
    public AlchemyRecipe recipeToCraft;
    public Toggle EnableRemove;
    private bool isInventoryOpen = false;
    public PlayerInteract playerInteract;
    public CurrencyManager currencyManager;
    public Text currencyText;
    public ShopInteraction shopInteraction;
    public GameObject SelectedSeedsPanel; // UI panel for selected seeds
    public Text SelectedSeedNameText;
    public Image SelectedSeedImage; // Drag your UI Image component here in Unity's editor
    public Text SelectedSeedCount; // UI text to display the currently selected seed
    public GameObject cropToBePlanted; // Selected crop prefab
    public Item currentSeed;
    private int currentSeedIndex = 0; // Keep track of the currently selected seed
    public List<Item> seedItems = new List<Item>(); 
    public List<Seeds> allSeeds;
    public delegate void InventoryChangedDelegate();
    public event InventoryChangedDelegate OnInventoryChanged;
    private void Awake()
    {
      
        Instance = this;
    }
    private void Start()
    {
      shopInteraction = FindObjectOfType<ShopInteraction>();
      ListItems();
    }

    private void Update()
    {
       HandleScrollInput();
    }
    public void SetRecipeToCraft(AlchemyRecipe newRecipe)
{
    recipeToCraft = newRecipe;
}

public void Add(Item item)
{
    Debug.Log($"Trying to add item with Name: {item.itemName}, Quantity: {item.quantity}");
    
    if (item.quantity <= 0)
    {
        return; // Don't add items with zero or negative quantity
    }

    if (item.isStackable)
    {
        Debug.Log("Item is Stackable");
        for (int i = 0; i < Items.Count; i++)
        {
            Item existingItem = Items[i];
            Debug.Log($"Found existing item with Quantity: {existingItem.itemName}, Item name: {item.itemName}");
            // Changed condition here from ID to itemName
            if (existingItem.id == item.id && existingItem.quantity < existingItem.maxStackCount)
            {
                Debug.Log($"Found existing item with Quantity: {existingItem.quantity}, MaxStack: {existingItem.maxStackCount}");
                
                int spaceLeftInStack = existingItem.maxStackCount - existingItem.quantity;
                int quantityToAdd = Mathf.Min(spaceLeftInStack, item.quantity);
                
                existingItem.quantity += quantityToAdd;
                Items[i] = existingItem; // Update the item in the list
                
                item.quantity -= quantityToAdd;
                
                if (item.quantity <= 0)
                {
                    return; // If all quantities are added, return
                }
            }
        }
    }

    if (item.quantity > 0)
    {
        Items.Add(item);
    }

    // Your other update methods...
    UpdateSeedItemsList();
    NotifyInventoryChanged();
}
  public void Remove(Item item)
{
    if (item.quantity <= 0)
    {
        Items.Remove(item);
        ListItems(); // Update the inventory UI after removing the item
    }
    if (item == currentSeed)
    {
        UpdateSelectedSeed(0); // or any other index based on your logic
    }
    
        Items.Remove(item);
        ListItems();
        UpdateSeedItemsList();
        NotifyInventoryChanged();
        
}

    public void ListItems()
    {
         seedItems.Clear();
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
            var removeButton = obj.transform.Find("RemoveButton").GetComponent<Button>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var sellButton = obj.transform.Find("SellButton").GetComponent<Button>();
            sellButton.onClick.RemoveAllListeners(); // Clear any previous listeners
            sellButton.onClick.AddListener(() => SellItemButtonClicked(item));

            itemName.text = item.itemName;
            itemIcon.sprite = item.itemIcon;

            if (item.isStackable)
            {
                var itemCountText = obj.transform.Find("ItemCount").GetComponent<Text>();
                itemCountText.text = "x" + item.quantity.ToString();
            }
            else
            {
                var itemCountText = obj.transform.Find("ItemCount").GetComponent<Text>();
                if (itemCountText != null)
                {
                    itemCountText.text = "";
                }
            }
             if (item.itemName.Contains("Seeds"))
            {
                seedItems.Add(item); // Add to our list of seed items
            }
            // Set the SellButton active based on the ShopUI state
            if (shopInteraction != null)
            {
                sellButton.gameObject.SetActive(shopInteraction.potionShopPanel.activeSelf);
            }
        }
         if (seedItems.Count > 0)
        {
            UpdateSelectedSeed(0);
        }
        else
        {
            // New code to handle when there are no seed items
            SelectedSeedNameText.text = "None";
            SelectedSeedImage.sprite = null;
            SelectedSeedCount.text = "";
            cropToBePlanted = null;
            currentSeed = null;
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

    public Item GetItemByID(string id)
    {
        foreach (var item in Items)
        {
            if (item.id == id)
            {
                return item;
            }
        }
        return null;
    }


  public bool CraftItem(AlchemyRecipe recipeToCraft, bool simulate = false)
{
    // Check if the selected recipe is null.
    if (recipeToCraft == null)
    {
        Debug.LogError("CraftItem called with a null recipe.");
        return false;
    }

    // Check if the player has all the required ingredients
    foreach (HarvestableCrop requiredIngredient in recipeToCraft.requiredIngredients)
    {
        string requiredItemName = requiredIngredient.itemName;
        int requiredQuantity = requiredIngredient.quantity;

        // Find the item in the player's inventory that matches the required ingredient
        Item matchingItem = Items.Find(item => item.itemName == requiredItemName);

        if (matchingItem == null || matchingItem.quantity < requiredQuantity)
        {
            Debug.LogWarning("Player doesn't have enough of the required ingredient: " + requiredItemName);
            return false; // Exit early because there's not enough of the required item.
        }
    }
    if (!simulate)
    {
    // Deduct the required quantities from the inventory
    foreach (HarvestableCrop requiredIngredient in recipeToCraft.requiredIngredients)
    {
        string requiredItemName = requiredIngredient.itemName;
        int requiredQuantity = requiredIngredient.quantity;

        Item matchingItem = Items.Find(item => item.itemName == requiredItemName);

        if (matchingItem != null)
        {
            matchingItem.quantity -= requiredQuantity;

            // If the item is no longer stackable, remove it from the inventory if its quantity reaches 0.
            if (matchingItem.quantity <= 0)
            {
                Remove(matchingItem);
                Debug.Log("Removed item: " + requiredItemName);
            }
        }
    }

    // Try to find the crafted item in the inventory
    Item craftedItem = Items.Find(item => item.id == recipeToCraft.resultingPotion.id);

    if (craftedItem != null)
    {
        // If the crafted item exists, increment its quantity by 1
        craftedItem.quantity++;
    }
    else
    {
        // If the crafted item doesn't exist, create a new one with quantity 1 and add it to the inventory
        Item newCraftedItem = recipeToCraft.resultingPotion;
        newCraftedItem.quantity = 1;
        Items.Add(newCraftedItem);
    }

    ListItems();
    NotifyInventoryChanged();
    return true; // Return true to indicate a successful crafting operation.
  
    }
    else
    {
        return true;
    }
}


    public void ToggleInventory(bool isOpen)
    {
        isInventoryOpen = isOpen;
    }

    public void BuyItem(Item item)
    {
        if (CurrencyManager.Instance.DeductCurrency(item.value))
        {
            Add(item);
            NotifyInventoryChanged();
        }
        else
        {
            Debug.LogWarning("Not enough currency to buy " + item.itemName);
        }

    }
    public void SellItem(Item item)
    {
    CurrencyManager.Instance.AddCurrency(item.value);
    Remove(item);
    }
    public void SellItemButtonClicked(Item item)
    {
    // Check if the ShopInteraction reference is not null
    if (shopInteraction != null)
    {
        // Call the SellShopItem method in ShopInteraction to transfer the item to the shop's inventory
        shopInteraction.SellShopItem(item);
        NotifyInventoryChanged();
        // Update the shop's UI to reflect the items in the shop's inventory
        //shopInteraction.UpdateShopUI();
    }
    else
    {
        Debug.LogWarning("ShopInteraction reference is null.");
    }
}
private void UpdateSelectedSeed(int index)
{
    currentSeedIndex = index;
    if (index >= 0 && index < seedItems.Count)
    {
        currentSeed = seedItems[index];
        SelectedSeedImage.sprite = currentSeed.itemIcon;
        if (currentSeed.isStackable)
        {
            SelectedSeedCount.text = "x" + currentSeed.quantity.ToString();
        }
        else
        {
            SelectedSeedCount.text = "";
        }
        Seeds correspondingSeed = FindSeedFromItem(currentSeed);
        if (correspondingSeed != null)
        {
            cropToBePlanted = correspondingSeed.cropToPlant;
        }

        // Update the displayed seed name
        if (SelectedSeedNameText != null && currentSeedIndex >= 0 && currentSeedIndex < seedItems.Count)
        {
            SelectedSeedNameText.text = seedItems[currentSeedIndex].itemName;
        }
    }
    if (seedItems.Count == 0)
        {
            SelectedSeedNameText.text = "None";
            SelectedSeedImage.sprite = null;
            SelectedSeedCount.text = "";
            cropToBePlanted = null;
            return;
        }
}
private void HandleScrollInput()
{
    float scroll = Input.GetAxis("Mouse ScrollWheel");
    if (scroll != 0)
    {
        if (scroll > 0) // scroll up
        {
            currentSeedIndex++;
        }
        else // scroll down
        {
            currentSeedIndex--;
        }

        // Ensure the index is within valid range
        currentSeedIndex = Mathf.Clamp(currentSeedIndex, 0, seedItems.Count - 1);

        // Update the selected seed
        UpdateSelectedSeed(currentSeedIndex);
    }
}

private Seeds FindSeedFromItem(Item item)
{
    // Assuming allSeeds is a List<Seeds> containing all possible Seeds objects.
    foreach (Seeds seed in allSeeds)
    {
        if (seed.itemName == item.itemName)
        {
            return seed;
        }
    }
    return null;
}
public void UpdateSeedItemsList()
{
    // Check if seedItems is null
    if (seedItems == null)
    {
        Debug.LogError("seedItems is null");
        return;
    }

    seedItems.Clear(); // Clear the existing list to rebuild it
    
    // Check if Items is null
    if (Items == null)
    {
        Debug.LogError("Items is null");
        return;
    }

    foreach (var item in Items)
    {
        // Check if itemName is null
        if (item.itemName == null)
        {
            Debug.LogError("itemName is null");
            continue;  // Skip to the next iteration
        }

        if (item.itemName.Contains("Seeds"))
        {
            seedItems.Add(item);
        }
    }
}

public void RemoveSingleQuantityOfItem(Item item)
{
    if (item.isStackable && item.quantity > 0)
    {
        item.quantity--;
        if (item.quantity <= 0)
        {
            Items.Remove(item);
        }
    }
    else
    {
        Items.Remove(item);
    }

    UpdateSeedItemsList(); // Update the list of seed items
    ListItems(); // Update the inventory UI if needed
}
public void NotifyInventoryChanged()
    {
        OnInventoryChanged?.Invoke();
    }

    
}