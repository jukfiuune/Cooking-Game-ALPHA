using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Recipe/Crafting")]
public class CraftingRecipes : ScriptableObject
{
    [Header("Recipe")]
    public Item product;
    public int amount;
    public Item[] Requirements;
    public int[] RequirementsAmount;

    public string category;
}
