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
    private GameObject lastHighlightedObject;
    public Text interactionPrompt;
    public GameObject InfoPanel;
    private GameObject currentInteractableObject;
    public SoundManager soundManager;
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
    
      GameObject nearestObject = GetNearestObject();

        // Highlight the nearest object
        HighlightNearestObject(nearestObject);
         if (nearestObject != null)
        {
            if (nearestObject.CompareTag("ShopSell"))
            {
                interactionPrompt.text = "E - Sell Potions";
            }
            else if (nearestObject.CompareTag("Shop"))
            {
                interactionPrompt.text = "E - Buy Seeds";
            }
            else if (nearestObject.CompareTag("AlchemyStation"))
            {
                interactionPrompt.text = "E - Brew Potions";
            }
            else if (nearestObject.CompareTag("Bed"))
            {
                interactionPrompt.text = "E - Sleep to Next Day";
            }
            else if (nearestObject.CompareTag("Shrine"))
            {
                interactionPrompt.text = "E - Make an Offering";
            }
            else if (nearestObject.CompareTag("Thorns"))
            {
                interactionPrompt.text = "E - Examine";
            }
            else if (nearestObject.CompareTag("ThornsGuy"))
            {
                interactionPrompt.text = "E - Talk";
            }
            else if (nearestObject.CompareTag("TeleportPad"))
            {
                TeleportPadScript teleportPad = nearestObject.GetComponent<TeleportPadScript>();
                if (teleportPad != null)
                {
                    if (!teleportPad.IsUnlocked())
                    {
                        interactionPrompt.text = "Double Tap E to Unlock";
                    }
                }
                else
                {
                    HubTeleportPadScript hubTeleportPad = nearestObject.GetComponent<HubTeleportPadScript>();
                    if (hubTeleportPad != null)
                    {
                        string destinationName = hubTeleportPad.GetCurrentDestinationName();
                        interactionPrompt.text = $"E - Change Destination (Current: {destinationName})";
                    }
                }
            }


            else if (nearestObject.CompareTag("Plantable"))
            {
                DirtPlotManager dirtPlotManager = nearestObject.GetComponent<DirtPlotManager>();
                if (dirtPlotManager != null)
                {
                    if (dirtPlotManager.isCropPlanted)
                    {
                        CropInteraction cropInteraction = dirtPlotManager.currentCrop.GetComponent<CropInteraction>();
                        if (cropInteraction != null && cropInteraction.IsFullyGrown())
                        {
                            interactionPrompt.text = "E - Harvest";
                        }
                        else
                        {
                            interactionPrompt.text = "";
                        }
                    }
                    else if (dirtPlotManager.IsPlantable())
                    {
                        interactionPrompt.text = "E - Plant Seeds";
                    }
                    else
                    {
                        interactionPrompt.text = "";
                    }
                }
            }

            else if (nearestObject.CompareTag("CropForPickup"))
            {
                CropInteraction cropInteraction = nearestObject.GetComponent<CropInteraction>();
                if (cropInteraction.IsFullyGrown())
                {
                interactionPrompt.text = "E - Harvest";
                }
                else
                {
                    interactionPrompt.text = "";
                }
            }
            else if (nearestObject.CompareTag("Info"))
            {
                interactionPrompt.text = "E - Read";
            }
            else
            {
                interactionPrompt.text = "";
            }
            
        }
        else
        {
            interactionPrompt.text = "";
        }
    // Check for player input to interact with objects
    if (Input.GetKeyDown(KeyCode.E))
    {
        // Perform interaction with the nearest object
        GameObject nearestObj = GetNearestObject();
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
            else if (nearestObject.CompareTag("Info"))
            {
                Debug.Log("Info object interacted with");
                OpenInfoPanel(nearestObject);
                currentInteractableObject = nearestObject;

                FloatingPrompt promptController = nearestObject.GetComponent<FloatingPrompt>();
                if (promptController != null)
                {
                    promptController.DisablePrompt();
                }
                else
                {
                    Debug.Log("FloatingPromptController not found on the object");
                }
            }


            else if (nearestObject.CompareTag("Shrine"))
            {
                HandleShrineInteraction(nearestObject);
            }
            else if (nearestObject.CompareTag("TeleportPad"))
            {
                HandleTeleportPadInteraction(nearestObject);
            }
            
            else if (nearestObject.CompareTag("Thorns"))
            {
                nearestObject.GetComponent<ThornsScript>().Interact();
            }
            else if (nearestObject.CompareTag("ThornsGuy"))
            {
                nearestObject.GetComponent<ThornsGuyDialogue>().TriggerDialogue();
            }
           
        }
    }
    if (InfoPanel.activeSelf && currentInteractableObject != null)
        {
            float distance = Vector3.Distance(transform.position, currentInteractableObject.transform.position);
            if (distance > interactionDistance)
            {
                CloseInfoPanel(); // Close the panel if the player is too far away
            }
        }
    
}


   private GameObject[] GetInteractableObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance);
        List<GameObject> interactableObjectList = new List<GameObject>();

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Interactable") || col.CompareTag("CropForPickup") || col.CompareTag("Plantable") || col.CompareTag("AlchemyStation") || col.CompareTag("ShopSell") || col.CompareTag("Bed") || col.CompareTag("Shop") || col.CompareTag("Info") || col.CompareTag("Shrine") || col.CompareTag("TeleportPad") || col.CompareTag("Thorns") || col.CompareTag("ThornsGuy"))
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

 private void HighlightNearestObject(GameObject nearestObj)
{
    // If a new nearest object is detected
    if (lastHighlightedObject != nearestObj)
    {
        // Remove existing highlights, if any
        if (lastHighlightedObject != null)
        {
            Outline outline = lastHighlightedObject.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }

            foreach (Transform child in lastHighlightedObject.transform)
            {
                Outline childOutline = child.GetComponent<Outline>();
                if (childOutline != null)
                {
                    childOutline.enabled = false;
                }
            }
        }

        // Apply new highlight only if the crop is fully grown
        if (nearestObj != null)
        {
            CropInteraction cropInteraction = nearestObj.GetComponent<CropInteraction>();
            if (cropInteraction == null || cropInteraction.IsFullyGrown())
            {
                // For single model objects
                Outline nearestOutline = nearestObj.GetComponent<Outline>();
                if (nearestOutline != null)
                {
                    nearestOutline.enabled = true;
                }

                // For multiple model objects like crops
                foreach (Transform child in nearestObj.transform)
                {
                    if (child.gameObject.activeInHierarchy)
                    {
                        Outline childOutline = child.GetComponent<Outline>();
                        if (childOutline == null)
                        {
                            childOutline = child.gameObject.AddComponent<Outline>();
                        }
                        childOutline.enabled = true;
                    }
                }
            }
        }

        // Update the last highlighted object
        lastHighlightedObject = nearestObj;
    }
}




    private void HandleInteractableInteraction(GameObject interactableObject)
    {
       Debug.Log("Interacting with " + gameObject.name);
    }

    private IEnumerator HandleInteractionWithDelay(GameObject interactableObject, Action interactionMethod)
{
    PlayerMovement.Instance.SetPlayerMovement(false); // Disable movement

    interactionMethod(); // Perform the interaction (planting or harvesting)

    yield return new WaitForSeconds(1); // Wait for 1 second

    PlayerMovement.Instance.SetPlayerMovement(true); // Re-enable movement
}

private void HandleCropInteraction(GameObject cropObject)
{
    CropInteraction cropInteraction = cropObject.GetComponent<CropInteraction>();

    if (cropInteraction != null)
    {
        if (cropInteraction.IsFullyGrown())
        {
            StartCoroutine(HandleInteractionWithDelay(cropObject, () => cropInteraction.Harvest()));
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
            StartCoroutine(HandleInteractionWithDelay(dirtPlotObject, () => dirtPlotManager.PlantCrop()));
        }
        else
        {
            StartCoroutine(HandleInteractionWithDelay(dirtPlotObject, () => dirtPlotManager.HarvestCrop()));
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
    soundManager.PlaySleepDing();

    // Get all crop objects in the scene
    CropInteraction[] cropInteractions = FindObjectsOfType<CropInteraction>();

    foreach (CropInteraction crop in cropInteractions)
    {
        // Increment the growth stage of each crop
        crop.IncrementGrowthByDay();
    }
    ObjectiveManager.Instance.CompleteObjective("snooze");
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

void OpenInfoPanel(GameObject infoObject)
{
    InfoPanel.SetActive(!InfoPanel.activeSelf);

}
 void CloseInfoPanel()
    {
        InfoPanel.SetActive(false);
        currentInteractableObject = null; // Clear the reference
    }
private Item FindItemByName(string itemName)
{
    foreach (var item in inventoryManager.Items)
    {
        if (item.itemName == itemName)
        {
            return item;
        }
    }
    return null; // Return null if the item is not found
}



private void HandleShrineInteraction(GameObject shrineObject)
{
    
    ShrineInteraction shrineInteraction = shrineObject.GetComponent<ShrineInteraction>();
    Item goldenDragonFruit = FindItemByName("Golden Dragon Fruit");

    Debug.Log("Shrine Interaction Check: " + (shrineInteraction != null));
    Debug.Log("Golden Dragon Fruit Found: " + (goldenDragonFruit != null));
    Debug.Log("Golden Dragon Fruit Quantity: " + (goldenDragonFruit != null ? goldenDragonFruit.quantity.ToString() : "N/A"));

    if (shrineInteraction != null && goldenDragonFruit != null && goldenDragonFruit.quantity > 0)
    {
        inventoryManager.Remove(goldenDragonFruit);
        shrineInteraction.Interact();
    }
    else
    {
        Debug.Log("You need a Golden Dragon Fruit to activate the shrine.");
    }
}
private void HandleTeleportPadInteraction(GameObject teleportPad)
{
    // First, try to get the script for a single destination pad
    TeleportPadScript padScript = teleportPad.GetComponent<TeleportPadScript>();
    if (padScript != null)
    {
        // If found, handle the interaction for a single destination pad
        padScript.HandleInteraction();
        return; // Exit the method to avoid further checks
    }

    // Next, try to get the script for the hub pad
    HubTeleportPadScript hubPadScript = teleportPad.GetComponent<HubTeleportPadScript>();
    if (hubPadScript != null)
    {
        // If found, handle the interaction for the hub pad
        hubPadScript.ChangeDestination();
    }
}



}



