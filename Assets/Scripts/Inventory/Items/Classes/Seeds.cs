using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Seeds", menuName = "Seeds/Create New Seeds")]
public class Seeds : ScriptableObject
{
    public string id;
    public string seedName;
    public string itemName;
    public Sprite itemIcon;
    public bool isStackable;
    public int quantity;
    public int value;
    public int maxStackCount;
    public GameObject cropToPlant; // Add this field to specify the crop prefab.
}
