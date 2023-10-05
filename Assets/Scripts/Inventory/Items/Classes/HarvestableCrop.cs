using UnityEngine;

[CreateAssetMenu(fileName = "New Harvestable Crop", menuName = "Harvestable Crop/Base Crop")]
public class HarvestableCrop : ScriptableObject
{
    public string cropName;
    public int growthTime;
    public GameObject[] growthStages; // Array of GameObjects representing different growth stages.
}