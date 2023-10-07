using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public float interactionDistance = 2f;
    private GameObject[] interactableObjects;
    public AlchemyStationCrafting alchemyStation;
    public Dropdown recipeDropdown;
    private List<AlchemyRecipe> availableRecipes = new List<AlchemyRecipe>();
    public List<AlchemyRecipe> individualRecipes;
    private int selectedRecipeIndex = 0;
    public Button craftingButton;
    public InventoryManager inventoryManager;
    private AlchemyRecipe selectedRecipe;
    public AlchemyRecipe recipeToCraft;
    public Animator animator;

   private void Start()
{
    // Copy the individual recipes to the availableRecipes list.
    availableRecipes = new List<AlchemyRecipe>(individualRecipes);
    craftingButton.onClick.AddListener(CraftButtonClicked);
    recipeDropdown.onValueChanged.AddListener(OnRecipeDropdownValueChanged);
    PopulateRecipeDropdown();

    // Initially select the first recipe (or another default if needed).
    if (availableRecipes.Count > 0)
    {
        selectedRecipeIndex = 0;
        selectedRecipe = availableRecipes[selectedRecipeIndex];
        recipeToCraft = selectedRecipe;
        recipeDropdown.value = selectedRecipeIndex;
        Debug.Log("Selected Recipe: " + recipeToCraft.name);
    }
}
private void PopulateRecipeDropdown()
{
    recipeDropdown.ClearOptions();

    List<string> recipeNames = new List<string>();
    foreach (AlchemyRecipe recipe in availableRecipes)
    {
        recipeNames.Add(recipe.name);
    }

    recipeDropdown.AddOptions(recipeNames);
}
     private void UpdateSelectedRecipe()
{
    // Update the selected recipe based on the dropdown's value.
    selectedRecipeIndex = recipeDropdown.value;

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
           
        }
    }
}


    private GameObject[] GetInteractableObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance);
        List<GameObject> interactableObjectList = new List<GameObject>();

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Interactable") || col.CompareTag("CropForPickup") || col.CompareTag("Plantable") || col.CompareTag("AlchemyStation"))
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
    DirtPlotInteraction dirtPlotInteraction = dirtPlotObject.GetComponent<DirtPlotInteraction>();

    // Check if the script component and tag are found
    if (dirtPlotInteraction != null && dirtPlotObject.CompareTag("Plantable") && dirtPlotInteraction.isPlantable)
    {
        // Call the appropriate method from DirtPlotInteraction
        dirtPlotInteraction.InteractWithDirtPlot();
        Debug.Log("Interacting with " + dirtPlotObject.name);
    }
    else
    {
        // Handle cases where the dirt plot is not plantable or doesn't have the script/tag
        Debug.LogWarning("Dirt plot cannot be planted or is missing components.");
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

    // Call the AlchemyStationCrafting's CraftItem method using the button click handler.

    // Now, call the HandleAlchemyStationInteraction method with only one argument.
    HandleAlchemyStationInteraction(alchemyStation.gameObject);
}
 public void OnRecipeDropdownValueChanged(int value)
{
    // Update the selected recipe based on the index
    if (value >= 0 && value < availableRecipes.Count)
    {
        // Update the selected recipe in the PlayerInteract script
        selectedRecipeIndex = value;
        selectedRecipe = availableRecipes[selectedRecipeIndex];
        recipeToCraft = selectedRecipe; // Update the recipeToCraft field

        Debug.Log("Selected Recipe (PlayerInteract): " + recipeToCraft.name);
    }
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
        inventoryManager.CraftItem(recipeToCraft);
    }
    else
    {
        Debug.LogWarning("No recipe selected for crafting.");
    }
}


}

