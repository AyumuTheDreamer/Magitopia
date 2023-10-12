using UnityEngine;

[System.Serializable]
public class GrowthStage
{
    public GameObject model;
    public float growthTime;
}

public class CropInteraction : MonoBehaviour
{
    // Reference to the specific crop type 
    public HarvestableCrop crop;
    public Animator animator;
    public InventoryManager inventoryManager;
    public GameObject immatureModel;
    public GameObject fullyGrownModel;
    public int harvestedQuantity = 5; // Initialize harvestedQuantity
    public GrowthStage[] growthStages; // Array of growth stages
    private int currentGrowthStage = 0; // Current growth stage index
    private float currentGrowthTime = 0f;
    
    private void Update()
    {
        if (currentGrowthStage < growthStages.Length)
        {
            currentGrowthTime += Time.deltaTime;
            if (currentGrowthTime >= growthStages[currentGrowthStage].growthTime)
            {
                // Move to the next growth stage
                currentGrowthStage++;
                
                if (currentGrowthStage == growthStages.Length)
                {
                    // Crop is fully grown
                    // Activate the fully grown model and deactivate the immature model
                    immatureModel.SetActive(false);
                    fullyGrownModel.SetActive(true);
                }
                else
                {
                    // Switch to the next growth stage model
                    growthStages[currentGrowthStage - 1].model.SetActive(false);
                    growthStages[currentGrowthStage].model.SetActive(true);
                    
                    // Reset the growth timer for the next stage
                    currentGrowthTime = 0f;
                }
            }
        }
    }

public bool IsFullyGrown()
{
    return currentGrowthStage == growthStages.Length;
}

public void Harvest()
{
    animator.SetTrigger("Interact");
    Debug.Log("Harvesting the crop: " + gameObject.name);

    if (inventoryManager != null)
    {
        // Create a new instance of the Item Scriptable Object
        Item cropItem = ScriptableObject.CreateInstance<Item>();
        cropItem.itemName = crop.cropName;
        cropItem.id = "Crop_" + crop.cropName;
        cropItem.itemIcon = crop.itemIcon;
        // Determine the harvested quantity based on the current growth stage
        if (IsFullyGrown())
        {
            cropItem.quantity = harvestedQuantity;
            Debug.Log("Harvesting fully grown crop. Quantity: " + harvestedQuantity);
        }
        else
        {
            cropItem.quantity = 0;
            Debug.Log("Harvesting immature crop. Quantity: 0");
        }

        cropItem.isStackable = true;
        cropItem.maxStackCount = 99;

        // Reset the growth timer and state
        currentGrowthStage = 0; // Reset to the first growth stage

        // Deactivate the fully grown model (if applicable) and activate the immature model
        foreach (var stage in growthStages)
        {
            stage.model.SetActive(false);
        }
        if (growthStages.Length > 0)
        {
            growthStages[0].model.SetActive(true);
        }

        // Add the crop item to the inventory
        inventoryManager.Add(cropItem);
    }
}
public void ResetGrowth()
{
    currentGrowthStage = 0;
    currentGrowthTime = 0f;
    
    // Deactivate all growth stage models except the first one
    for (int i = 1; i < growthStages.Length; i++)
    {
        growthStages[i].model.SetActive(false);
    }
}

}