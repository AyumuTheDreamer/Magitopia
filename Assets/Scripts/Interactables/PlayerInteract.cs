using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public float interactionDistance = 2f;
    private GameObject[] interactableObjects;
    public AlchemyStationCrafting alchemyStation;
    private List<AlchemyRecipe> availableRecipes = new List<AlchemyRecipe>();
    public List<AlchemyRecipe> individualRecipes;
    private int selectedRecipeIndex = 0;
    public Button craftingButton;
    public InventoryManager inventoryManager;
    private AlchemyRecipe selectedRecipe;
    public AlchemyRecipe recipeToCraft;
    public Animator animator;
    public Button sellButton;
    public DirtPlotManager dirtPlotManager;
    public GameObject cropPrefab;
    public ShopInteraction shopInteraction;
    public TimeController timeController;
    public GameObject ingredientPrefab;
    public SeedShop seedShop;
   private void Start()
{
    // Copy the individual recipes to the availableRecipes list.
    availableRecipes = new List<AlchemyRecipe>(individualRecipes);

    // Initially select the first recipe (or another default if needed).
    if (availableRecipes.Count > 0)
    {
        selectedRecipe = availableRecipes[selectedRecipeIndex];
        recipeToCraft = selectedRecipe;
        Debug.Log("Selected Recipe: " + recipeToCraft.name);
    }
    
}

     private void UpdateSelectedRecipe()
{

    if (selectedRecipeIndex >= 0 && selectedRecipeIndex < availableRecipes.Count)
    {
        selectedRecipe = availableRecipes[selectedRecipeIndex];
    }
}

    private void Update()
{
    // Check for nearby objects
    interactableObjects = GetInteractableObjects();
    
    

    // Check for player input to interact with objects
    if (Input.GetKeyDown(KeyCode.E))
    {
        // Perform interaction with the nearest object
        GameObject nearestObject = GetNearestObject();
        if (nearestObject != null)
        {
            if (nearestObject.CompareTag("Interactable"))
            {
                // Handle interaction with objects tagged as "Interactable"
                HandleInteractableInteraction(nearestObject);
            }
            else if (nearestObject.CompareTag("CropForPickup"))
            {
                // Handle interaction with objects tagged as "CropForPickup"
                HandleCropInteraction(nearestObject);
            }
            else if (nearestObject.CompareTag("Plantable"))
            {
                HandleDirtPlotInteraction(nearestObject);
            }
            else if (nearestObject.CompareTag("AlchemyStation"))
            {
            // Handle interaction with objects tagged as "AlchemyStation"
                HandleAlchemyStationInteraction(nearestObject);
            }
            else if (nearestObject.CompareTag("ShopSell"))
            {
                HandleShopSellInteraction(nearestObject);
                
            }
            else if(nearestObject.CompareTag("Bed"))
            {
                Sleep(nearestObject);
            }
             else if (nearestObject.CompareTag("Shop"))
            {
            HandleSeedShopInteraction(nearestObject);
            }   

           
        }
    }
}


    private GameObject[] GetInteractableObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance);
        List<GameObject> interactableObjectList = new List<GameObject>();

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Interactable") || col.CompareTag("CropForPickup") || col.CompareTag("Plantable") || col.CompareTag("AlchemyStation") || col.CompareTag("ShopSell") || col.CompareTag("Bed") || col.CompareTag("Shop"))
            {
                interactableObjectList.Add(col.gameObject);
            }
        }

        return interactableObjectList.ToArray();
    }

    private GameObject GetNearestObject()
    {
        GameObject nearestObject = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject obj in interactableObjects)
        {
            if (obj != null)
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < nearestDistance)
                {
                    nearestObject = obj;
                    nearestDistance = distance;
                }
            }
        }

        return nearestObject;
    }

    private void HandleInteractableInteraction(GameObject interactableObject)
    {
       Debug.Log("Interacting with " + gameObject.name);
    }

    private void HandleCropInteraction(GameObject cropObject)
{
    CropInteraction cropInteraction = cropObject.GetComponent<CropInteraction>();

    if (cropInteraction != null)
    {
        if (cropInteraction.IsFullyGrown())
        {
            // Crop is fully grown and can be harvested
            cropInteraction.Harvest();
        }
        else
        {
            Debug.Log("Crop is not fully grown yet.");
        }
    }
}
    private void HandleDirtPlotInteraction(GameObject dirtPlotObject)
{
    DirtPlotManager dirtPlotManager = dirtPlotObject.GetComponent<DirtPlotManager>();

    if (dirtPlotManager != null)
    {
        if (dirtPlotManager.IsPlantable())
        {
            // Plant a crop in the dirt plot
            dirtPlotManager.PlantCrop();
        }
        else
        {
            // Harvest the crop from the dirt plot
            dirtPlotManager.HarvestCrop();
        }
    }
}

private void HandleAlchemyStationInteraction(GameObject alchemyStationObject)
    {
        if (alchemyStationObject != null)
        {
            // Open the UI for alchemy station
            alchemyStation.Interact();
            inventoryManager.ListItems();
            // You can also close the UI if it's open by calling alchemyStation.Interact() again.
        }
        else
        {
            Debug.LogWarning("AlchemyStationCrafting component not found.");
        }
    }

private void InteractWithAlchemyStation()
{
    // Set the recipe to craft in the AlchemyStationCrafting script.
    alchemyStation.recipeToCraft = selectedRecipe;

    // Now, call the HandleAlchemyStationInteraction method with only one argument.
    HandleAlchemyStationInteraction(alchemyStation.gameObject);
}
private void SetRecipeForCrafting(AlchemyRecipe newRecipe)
    {
        inventoryManager.SetRecipeToCraft(newRecipe);
    }

public void CraftButtonClicked()
{
    // Check if recipeToCraft is not null before crafting.
    if (recipeToCraft != null)
    {
        DisplayIngredients(recipeToCraft);
        inventoryManager.CraftItem(recipeToCraft);
    }
    else
    {
        Debug.LogWarning("No recipe selected for crafting.");
    }
}
void DisplayIngredients(AlchemyRecipe recipe)
    {
        // Clear any existing displayed ingredients
        foreach (Transform child in craftingButton.transform)
        {
            Destroy(child.gameObject);
        }

        // Loop through each ingredient in the recipe
        foreach (HarvestableCrop ingredient in recipe.requiredIngredients)
        {
            // Instantiate the ingredientPrefab and set its parent to the crafting button
            GameObject instantiatedPrefab = Instantiate(ingredientPrefab, craftingButton.transform);

            // Find and set the Image and Text components of the instantiated prefab
            Image ingredientImage = instantiatedPrefab.GetComponentInChildren<Image>();
            Text ingredientText = instantiatedPrefab.GetComponentInChildren<Text>();

            if (ingredientImage != null && ingredientText != null)
            {
                ingredientImage.sprite = ingredient.itemIcon; // The sprite field should be a part of your Ingredient scriptable object
                ingredientText.text = ingredient.quantity.ToString(); // Assuming you have a quantity field in your Ingredient scriptable object
            }
            else
            {
                Debug.LogWarning("Image or Text component not found on the ingredientPrefab.");
            }
        }
    }
  private void HandleShopSellInteraction(GameObject shopObject)
{
    ShopInteraction shopInteraction = shopObject.GetComponent<ShopInteraction>();

    if (shopInteraction != null)
    {
        // Call the Interact method on the ShopInteraction script
        shopInteraction.Interact();
    }
    else
    {
        Debug.LogWarning("ShopInteraction component not found on the ShopSell object.");
    }
}
private void Sleep(GameObject bedObject)
{
    if (timeController == null)
    {
        Debug.LogError("timeController is null");
        return;
    }
    else
    {
        Debug.Log("Incrementing growth");
    }
    // Get the current time
    DateTime currentTime = timeController.GetCurrentTime();

    // Set the time to 7 AM
    currentTime = currentTime.Date + new TimeSpan(7, 0, 0);

    // Increment the day counter
    int currentDay = timeController.GetDayCounter();
    timeController.dayCounter = currentDay + 1;

    // Update the time controller with the new time
    timeController.SetCurrentTime(7, 0);

    // Get all crop objects in the scene
    CropInteraction[] cropInteractions = FindObjectsOfType<CropInteraction>();

    foreach (CropInteraction crop in cropInteractions)
    {
        // Increment the growth stage of each crop
        crop.IncrementGrowthByDay();
    }
}
private void HandleSeedShopInteraction(GameObject shopObject)
{
    SeedShop seedShop = shopObject.GetComponent<SeedShop>();

    if (seedShop != null)
    {
        seedShop.ToggleShopUI();
    }
    else
    {
        Debug.LogWarning("SeedShop component not found on the Shop object.");
    }
}


}




