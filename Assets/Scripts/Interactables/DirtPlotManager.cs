using UnityEngine;

public class DirtPlotManager : MonoBehaviour
{
    
    public bool isPlantable = true; // Flag to check if the dirt plot is plantable.
    public GameObject currentCrop; // Reference to the planted crop.
    public bool isCropPlanted = false;
    public InventoryManager inventoryManager; 
    public Animator animator;
    public SoundManager soundManager;
    public ObjectiveManager objectiveManager;
    public bool IsPlantable()
    {
        return isPlantable && !isCropPlanted;
    }

   public void PlantCrop()
    {
        if (inventoryManager.cropToBePlanted == null)
            {
                return;
            }
        if (IsPlantable())
        {
            GameObject cropPrefab = inventoryManager.cropToBePlanted;
            if(cropPrefab != null){
            // Generate a random Y-axis rotation angle between 0 and 360 degrees.
            float randomYRotation = Random.Range(0f, 360f);

            // Combine the original rotation with the random Y-axis rotation.
            Quaternion combinedRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, randomYRotation, transform.rotation.eulerAngles.z);

            currentCrop = Instantiate(cropPrefab, transform.position, combinedRotation);
            isCropPlanted = true;
            animator.SetTrigger("Plant");
            soundManager.PlayPlantingSound();
            inventoryManager.RemoveSingleQuantityOfItem(inventoryManager.currentSeed);
            objectiveManager.CompleteObjective("plantSeed");
            CropInteraction cropInteraction = currentCrop.GetComponent<CropInteraction>();
            if (cropInteraction != null)
            {
                cropInteraction.inventoryManager = inventoryManager;
                cropInteraction.animator = this.animator;
            }

            
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