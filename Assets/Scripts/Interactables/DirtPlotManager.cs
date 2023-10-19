using UnityEngine;

public class DirtPlotManager : MonoBehaviour
{
    
    public bool isPlantable = true; // Flag to check if the dirt plot is plantable.
    private GameObject currentCrop; // Reference to the planted crop.
    public bool isCropPlanted = false;
    public Animator cropAnimator; // Assign the animator in the Inspector.
    public InventoryManager inventoryManager; // Assign the Inventory Manager in the Inspector.

    public bool IsPlantable()
    {
        return isPlantable && !isCropPlanted;
    }

   public void PlantCrop()
{
    if (IsPlantable())
    {
        GameObject cropPrefab = inventoryManager.cropToBePlanted; // Take the crop to be planted from the InventoryManager
        currentCrop = Instantiate(cropPrefab, transform.position, transform.rotation);
        isCropPlanted = true;

        // Decrease the quantity of the currently selected seed
        inventoryManager.RemoveSingleQuantityOfItem(inventoryManager.currentSeed);

        // Assign the Animator and Inventory Manager to the newly instantiated crop.
        CropInteraction cropInteraction = currentCrop.GetComponent<CropInteraction>();
        if (cropInteraction != null)
        {
            cropInteraction.animator = cropAnimator;
            cropInteraction.inventoryManager = inventoryManager;
        }
    }
}




    public void HarvestCrop()
    {
        if (isCropPlanted)
        {
            CropInteraction cropInteraction = currentCrop.GetComponent<CropInteraction>();
            if (cropInteraction != null && cropInteraction.IsFullyGrown())
            {
                cropInteraction.Harvest();
                 
            }
            else if (cropInteraction != null && !cropInteraction.IsFullyGrown())
            {
                return;
            }

            Destroy(currentCrop);
            currentCrop = null;
            isCropPlanted = false; 
        }
    }
}