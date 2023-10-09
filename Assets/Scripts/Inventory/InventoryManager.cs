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
    public AlchemyRecipe recipeToCraft;
    public Toggle EnableRemove;
    private bool isInventoryOpen = false;
    public PlayerInteract playerInteract;

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
    public void SetRecipeToCraft(AlchemyRecipe newRecipe)
{
    recipeToCraft = newRecipe;
}

  public void Add(Item item)
{
    if (item.isStackable)
    {
        // Check if there's an existing stack of this item in the inventory.
        Item existingItem = Items.Find(i => i.id == item.id);

        if (existingItem != null && existingItem.quantity < existingItem.maxStackCount)
        {
            // Calculate the remaining space in the existing stack.
            int spaceLeftInStack = existingItem.maxStackCount - existingItem.quantity;

            // Calculate how much can be added to the stack.
            int quantityToAdd = Mathf.Min(spaceLeftInStack, item.quantity);

            // Increment the quantity of the existing stack.
            existingItem.quantity += quantityToAdd;

            // Reduce the quantity to be added.
            item.quantity -= quantityToAdd;

            // Exit the method if the item quantity is now 0.
            if (item.quantity == 0)
            {
                return;
            }
        }
    }

    // If the item is not stackable or no stack was found, add it as a new item.
    Items.Add(item);
}

  public void Remove(Item item)
{
    if (item.quantity <= 0)
    {
        Items.Remove(item);
        ListItems(); // Update the inventory UI after removing the item
    }
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
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
           

            itemName.text = item.itemName;
            itemIcon.sprite = item.itemIcon;


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


  public bool CraftItem(AlchemyRecipe recipeToCraft)
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

    return true; // Return true to indicate a successful crafting operation.
}




    public void ToggleInventory(bool isOpen)
    {
        isInventoryOpen = isOpen;
    }
}