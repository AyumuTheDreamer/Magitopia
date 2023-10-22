using UnityEngine;

public class DirtPlotManager : MonoBehaviour
{
    
    public bool isPlantable = true; // Flag to check if the dirt plot is plantable.
    private GameObject currentCrop; // Reference to the planted crop.
    public bool isCropPlanted = false;
    public InventoryManager inventoryManager; 
    public Animator animator;

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
        animator.SetTrigger("Plant");
        // Decrease the quantity of the currently selected seed
        inventoryManager.RemoveSingleQuantityOfItem(inventoryManager.currentSeed);

        // Assign the Animator and Inventory Manager to the newly instantiated crop.
        CropInteraction cropInteraction = currentCrop.GetComponent<CropInteraction>();
        if (cropInteraction != null)
        {
            cropInteraction.inventoryManager = inventoryManager;
        }
        if (inventoryManager.cropToBePlanted == null)
        {
        return;
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