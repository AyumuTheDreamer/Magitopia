using UnityEngine;

public class DirtPlotManager : MonoBehaviour
{
    public GameObject cropPrefab; // Reference to the crop prefab you want to plant.
    public bool isPlantable = true; // Flag to check if the dirt plot is plantable.
    private GameObject currentCrop; // Reference to the planted crop.
    public bool isCropPlanted = false;
    public Animator cropAnimator; // Assign the animator in the Inspector.
    public InventoryManager cropInventoryManager; // Assign the Inventory Manager in the Inspector.

    public bool IsPlantable()
    {
        return isPlantable && !isCropPlanted;
    }

    public void PlantCrop()
    {
        if (IsPlantable())
        {
            // Instantiate the crop prefab at the dirt plot's position and rotation.
            currentCrop = Instantiate(cropPrefab, transform.position, transform.rotation);
            isCropPlanted = true; // Mark the plot as having a crop planted.

            // Assign the Animator and Inventory Manager to the newly instantiated crop.
            CropInteraction cropInteraction = currentCrop.GetComponent<CropInteraction>();
            if (cropInteraction != null)
            {
                cropInteraction.animator = cropAnimator;
                cropInteraction.inventoryManager = cropInventoryManager;
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