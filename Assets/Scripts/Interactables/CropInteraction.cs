using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

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
    [SerializeField]
    private int initialGrowthStage = 0;
    public SoundManager soundManager;
    private void Start()
    {
        timeController = FindObjectOfType<TimeController>();
        if (timeController == null)
        {
            Debug.LogWarning("TimeController not found in the scene.");
        }
        lastGrowthUpdateTime = timeController.GetCurrentTime();

        // Set the initial growth stage
        currentGrowthStage = initialGrowthStage;

        // Initialize time until next stage
        if (currentGrowthStage < growthStages.Length)
        {
            timeUntilNextStage = growthStages[currentGrowthStage].growthTime;
        }

        // Set initial models based on growth stage
        UpdateCropModel();
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
        cropItem.isStackable = true;
        cropItem.maxStackCount = 99;

        if (IsFullyGrown())
        {
            cropItem.quantity = harvestedQuantity;

            // Handle specific crop objectives
            if (crop.cropName == "Heartfelt Berry")
            {
                ObjectiveManager.Instance.CompleteObjective("pickBerry");
            }
            else if (crop.cropName == "Drake Claw Plant")
            {
                ObjectiveManager.Instance.CompleteObjective("harvestClaw");
            }

            // Reset the growth stage and day counter upon harvesting a fully grown crop.
            currentGrowthStage = 0;
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

        inventoryManager.Add(cropItem);
        inventoryManager.ListItems();
        SoundManager.Instance.PlayHarvestingSound();
    }
}


private void UpdateCropModel()
    {
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

     public void IncrementGrowthByDay()
    {
        if (currentGrowthStage < growthStages.Length - 1)
        {
            currentGrowthStage += 1;
            UpdateCropModel();
        }
    }

}