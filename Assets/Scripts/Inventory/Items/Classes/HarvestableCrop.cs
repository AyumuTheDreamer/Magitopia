using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Harvestable Crop", menuName = "Harvestable Crop/Base Crop")]
public class HarvestableCrop : ScriptableObject
{   

    public string cropID; // Match this with your actual ID variable.
    public int quantity; // Match this with your actual quantity variable.
    public bool isStackable; // Match this with your actual isStackable variable.
    public string id;
    public string cropName;
    public string itemName;
    public int growthTime;
    public GameObject[] growthStages; // Array of GameObjects representing different growth stages.

    
    
}