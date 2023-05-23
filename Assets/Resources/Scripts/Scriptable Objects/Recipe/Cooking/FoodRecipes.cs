using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public abstract class FoodRecipes : ScriptableObject
{
    [Header("Recipe")]
    public string Name;
    public Item product;
    public int VeggiePoint = 0;
    public int FruitPoint = 0;
    public int MeatPoint = 0;
    public int SugarPoint = 0;
    public int DairyPoint = 0;
    public Item[] SpecialRequirements;
    public int[] SpecialRequirementsAmount;

    public int priority;

    public float cookTime;


}
