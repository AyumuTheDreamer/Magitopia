using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item",menuName ="Item/Create New Item")]
public class Item : ScriptableObject
{
    public string id; // Match this with your actual ID variable.
    public string itemName;
    public int value;
    public Sprite icon;
    public int maxStackCount;
    public int quantity; // Match this with your actual quantity variable.
    public bool isStackable; // Match this with your actual isStackable variable.

    public bool CanCraftFromRecipe(AlchemyRecipe recipe)
    {
        // Check if this item matches the recipe's resulting potion
        return recipe.resultingPotion == this;
    }
}
