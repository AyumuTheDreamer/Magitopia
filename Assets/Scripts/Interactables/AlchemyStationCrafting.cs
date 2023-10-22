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
    public GameObject recipeButtonContainer;
    public Button recipeButtonPrefab; 
    public AlchemyRecipe recipeToCraft;
    public GameObject ingredientDisplayPrefab;
    [SerializeField]
    private List<Button> recipeButtons = new List<Button>();
    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found in the scene!");
        }
        inventoryManager.OnInventoryChanged += CheckRecipeAvailability;
        // Populate the recipeButtonContainer with buttons
        PopulateRecipeButtons();
        CheckRecipeAvailability();
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
    }

  private void PopulateRecipeButtons()
{
    foreach (AlchemyRecipe recipe in recipes)
    {
        // Create the recipe button
        Button newButton = Instantiate(recipeButtonPrefab, recipeButtonContainer.transform);
        newButton.GetComponentInChildren<Text>().text = recipe.name;
        newButton.onClick.AddListener(() => CraftItemDirectly(recipe, newButton));

        // Initialize a position offset for ingredients
        float xOffset = 0;
        recipeButtons.Add(newButton);
        // Populate the button with ingredient information
        foreach (HarvestableCrop ingredient in recipe.requiredIngredients)
        {
            // Create an ingredient display and parent it to the button
            GameObject newIngredientDisplay = Instantiate(ingredientDisplayPrefab, newButton.transform);

            // Set the sprite and text of the ingredient display
            newIngredientDisplay.GetComponentInChildren<Image>().sprite = ingredient.itemIcon;
            newIngredientDisplay.GetComponentInChildren<Text>().text = "x" + ingredient.quantity.ToString();

            // Set the position of the ingredient display
            RectTransform rectTransform = newIngredientDisplay.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(xOffset, 0);
            
            // Increment the position offset for the next ingredient
            xOffset += 100; // Change this value to adjust the spacing
        }
    }
    
}



    public void SetRecipeToCraft(AlchemyRecipe recipe)
    {
        // Set the recipe to craft
        recipeToCraft = recipe;
        Debug.Log("Selected Recipe To Craft: " + recipeToCraft.name);
    }
    public void CheckRecipeAvailability()
    {
        for (int i = 0; i < recipes.Count; i++)
    {
        AlchemyRecipe recipe = recipes[i];
        Button correspondingButton = recipeButtons[i]; // Access the button from the list

        if(correspondingButton.CompareTag("RecipeButton")) // Check if the button has the tag
        {
            bool canCraft = inventoryManager.CraftItem(recipe, true); // Simulate crafting
            correspondingButton.interactable = canCraft; // Update the button's interactable state based on simulation result

            // Your existing code to display ingredients
        }
    }
    }
    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        inventoryManager.OnInventoryChanged -= CheckRecipeAvailability;
    }
   public void CraftItemDirectly(AlchemyRecipe recipe, Button correspondingButton)
{
    if (inventoryManager.CraftItem(recipe))  // Assuming CraftItem returns true if successful
    {
        Debug.Log("Successfully crafted: " + recipe.name);
        
        // Any additional logic like updating UI can go here.
        CheckRecipeAvailability(); // Update availability after crafting
    }
    else
    {
        Debug.LogWarning("Couldn't craft: " + recipe.name);
        // Maybe display a message to the player
    }
}


}

