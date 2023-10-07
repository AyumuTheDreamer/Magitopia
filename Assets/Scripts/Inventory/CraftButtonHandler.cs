using UnityEngine;
using UnityEngine.UI;

public class CraftButtonHandler : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public AlchemyRecipe recipeToCraft; // Optionally, you can set this in the Unity Editor if needed.

    public void CraftButtonClicked()
{
    // Check if recipeToCraft is not null before crafting.
    if (recipeToCraft != null)
    {
        bool craftingSuccess = inventoryManager.CraftItem(recipeToCraft);
        
        if (craftingSuccess)
        {
            // Craft the item and update the UI or perform other actions as needed.
            Debug.Log("Crafting: " + recipeToCraft.resultingPotion.itemName);
        }
        else
        {
            Debug.LogWarning("Crafting failed. Check if you have the required ingredients.");
        }
    }
    else
    {
        Debug.LogWarning("No recipe selected for crafting.");
    }
}
}
