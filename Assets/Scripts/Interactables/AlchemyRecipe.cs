using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Alchemy Recipe", menuName = "Alchemy/Recipe")]
public class AlchemyRecipe : ScriptableObject
{
    public List<HarvestableCrop> requiredIngredients;
    public Item resultingPotion;
}
