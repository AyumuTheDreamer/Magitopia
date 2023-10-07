using UnityEngine;
using UnityEngine.UI;

public class CraftButtonHandler : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public AlchemyRecipe recipeToCraft; // Optionally, you can set this in the Unity Editor if needed.

    public void OnCraftButtonClick()
    {
        // Debug log to check if recipeToCraft is correctly assigned
        Debug.Log("Recipe to Craft: " + (recipeToCraft != null ? recipeToCraft.name : "null"));

        // Call the CraftItem method in the InventoryManager
        if (inventoryManager != null)
        {
            bool craftingResult = inventoryManager.CraftItem(recipeToCraft);

            if (craftingResult)
            {
                Debug.Log("Potion Made");
            }
            else
            {
                Debug.LogWarning("Crafting Failed");
            }
        }
        else
        {
            Debug.LogError("InventoryManager not assigned to CraftButtonHandler.");
        }
    }
}
