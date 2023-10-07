using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AlchemyStationCrafting : MonoBehaviour
{
    public GameObject alchemyPanel;
    public GameObject inventoryPanel;
    public InventoryManager inventoryManager;
    private PlayerMovement playerMovement;
    public List<AlchemyRecipe> recipes;
    public Dropdown dropdown; // Reference to the Dropdown component.
    public AlchemyRecipe recipeToCraft;

    private void Start()
    {
        // Find the PlayerMovement script in the scene
        playerMovement = FindObjectOfType<PlayerMovement>();

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found in the scene!");
        }
            dropdown.onValueChanged.AddListener(OnRecipeDropdownValueChanged);
        
    }

    public void Interact()
    {
        Debug.Log("Interact method called on " + gameObject.name);
        // Your existing interaction logic here

        alchemyPanel.SetActive(!alchemyPanel.activeSelf);
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        playerMovement.isInventoryOpen = inventoryPanel.activeSelf;

        if (inventoryPanel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Re-lock the cursor when the inventory is closed
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Get the selected recipe from the dropdown.
        int selectedRecipeIndex = dropdown.value;
        if (selectedRecipeIndex >= 0 && selectedRecipeIndex < recipes.Count)
        {
            // Update the selected recipe based on the dropdown's value.
            recipeToCraft = recipes[selectedRecipeIndex];
        }
    }

    private void OnRecipeDropdownValueChanged(int value)
    {
        // Update the selected recipe based on the dropdown's value.
        if (value >= 0 && value < recipes.Count)
        {
            recipeToCraft = recipes[value];
        }
    }
    public void SetRecipeToCraft(AlchemyRecipe recipe)
{
    // Set the recipe to craft
    recipeToCraft = recipe;
    Debug.Log("Selected Recipe To Craft: " + recipeToCraft.name); // Add this line
}
}
