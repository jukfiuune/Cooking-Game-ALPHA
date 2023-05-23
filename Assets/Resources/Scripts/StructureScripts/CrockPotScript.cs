using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrockPotScript : MonoBehaviour
{
    Inventory inv;

    public FoodRecipes[] recipes;
    public int VeggiePoint = 0;
    public int FruitPoint = 0;
    public int MeatPoint = 0;
    public int SugarPoint = 0;
    public int DairyPoint = 0;
    public FoodRecipes selectedRecipe = null;
    public CrockpotStructure crockpot;

    public float secondsToCook = 5;
    public bool timerStarted = false;

    public int excess;

    void Start() {
        inv = GameObject.Find("UIOpen").GetComponent<OpenedUI>().openedInventory;
        crockpot = inv.transform.GetComponent<CrockpotStructure>();
        recipes = Resources.LoadAll<FoodRecipes>("Recipes/CrockPot");
    }

    public void Cook(){
        ResetCrockPot();
        // Adding points up
        for(int i = 0; i < inv.inventorySlots; i++){
            if(inv.items[i] != null){
                string type = inv.items[i].GetFood().type;
                int points = inv.items[i].GetFood().points;
                if      (type == "Veggie") VeggiePoint += points;
                else if (type == "Fruit")  FruitPoint  += points;
                else if (type == "Meat")   MeatPoint   += points;
                else if (type == "Sugar")  SugarPoint  += points;
                else if (type == "Dairy")  DairyPoint  += points;
            }
            else
            {
                return;
            }
        }
        
        // Searching for a recipe
        for (int recipeIndex = 0; recipeIndex < recipes.Length; recipeIndex++)
        {
            FoodRecipes cR = recipes[recipeIndex]; // CurrentRecipe
            bool allSpecialIngrFound = true;
            for (int specialIngr = 0; specialIngr < cR.SpecialRequirements.Length; specialIngr++)
            {

                int found = 0;
                int needed = cR.SpecialRequirementsAmount[specialIngr];
                Item item = cR.SpecialRequirements[specialIngr];
                for (int i = 0; i < inv.inventorySlots; i++)
                {
                    if (inv.items[i].Name == item.Name)
                    {
                        found++;
                    }
                }
                if (found < needed)
                {
                    allSpecialIngrFound = false;
                    break;
                }

            }
            if (allSpecialIngrFound)
            {
                if (cR.VeggiePoint <= VeggiePoint && cR.FruitPoint <= FruitPoint && cR.MeatPoint <= MeatPoint && cR.SugarPoint <= SugarPoint && cR.DairyPoint <= DairyPoint)
                {
                    if (selectedRecipe == null || selectedRecipe.priority < cR.priority)
                    {
                        selectedRecipe = cR;
                    }
                }
            }
        }
        // A recipe is found
        if (selectedRecipe != null)
        {
            crockpot.StartCooking(selectedRecipe.product, selectedRecipe.cookTime);
            inv.ClearInventory();
            inv.transform.GetComponent<InventoryInteract>().CloseInventory();
        }
    }
    public void ResetCrockPot(){
        selectedRecipe = null;
        VeggiePoint = 0;
        FruitPoint = 0;
        MeatPoint = 0;
        SugarPoint = 0;
        DairyPoint = 0;
    }
}
