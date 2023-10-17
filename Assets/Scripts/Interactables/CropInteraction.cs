using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class GrowthStage
{
    public GameObject model;
    public float growthTime;
}

public class CropInteraction : MonoBehaviour
{
    public HarvestableCrop crop;
    public Animator animator;
    public InventoryManager inventoryManager;
    public int harvestedQuantity = 5;
    public GrowthStage[] growthStages;
    public int currentGrowthStage = 0;
    public TimeController timeController;
    private float timeUntilNextStage;
    private DateTime lastGrowthUpdateTime;
    

    private void Start()
    {
        timeUntilNextStage = growthStages[currentGrowthStage].growthTime;

        // Find the TimeController in the scene and store the reference
        timeController = FindObjectOfType<TimeController>();
        if (timeController == null)
        {
            Debug.LogWarning("TimeController not found in the scene.");
        }
        lastGrowthUpdateTime = timeController.GetCurrentTime();
    }

    private void Update()
    {
        
    }


    public bool IsFullyGrown()
    {
        return currentGrowthStage >= growthStages.Length - 1;
    }

    public void Harvest()
{
    animator.SetTrigger("Interact");
    Debug.Log("Harvesting the crop: " + gameObject.name);

    if (inventoryManager != null)
    {
        Item cropItem = ScriptableObject.CreateInstance<Item>();
        cropItem.itemName = crop.cropName;
        cropItem.id = "Crop_" + crop.cropName;
        cropItem.itemIcon = crop.itemIcon;

         if (IsFullyGrown())
        {
            cropItem.quantity = harvestedQuantity;
            // Reset the growth stage and day counter upon harvesting a fully grown crop.
            currentGrowthStage = 0;

            // Deactivate the fully grown model and activate the immature model.
            foreach (var stage in growthStages)
            {
                stage.model.SetActive(false);
            }
            if (growthStages.Length > 0)
            {
                growthStages[0].model.SetActive(true);
            }
            Debug.Log("Harvesting fully grown crop. Quantity: " + harvestedQuantity);
        }
        else
        {
            cropItem.quantity = 0;
            Debug.Log("Harvesting immature crop. Quantity: 0");
        }

        cropItem.isStackable = true;
        cropItem.maxStackCount = 99;

        inventoryManager.Add(cropItem);
        inventoryManager.ListItems();
    }
}



    public void IncrementGrowthByDay()
    {
        // Check if there's a growth stage to increment to
        if (currentGrowthStage < growthStages.Length - 1)
        {
            // Calculate the next growth stage
            int nextGrowthStage = currentGrowthStage + 1;

            // Update the current growth stage
            currentGrowthStage = nextGrowthStage;

            // Handle model activation and deactivation here
            for (int i = 0; i < growthStages.Length; i++)
            {
                if (i == currentGrowthStage)
                {
                    growthStages[i].model.SetActive(true);
                }
                else
                {
                    growthStages[i].model.SetActive(false);
                }
            }
        }
    }

}
