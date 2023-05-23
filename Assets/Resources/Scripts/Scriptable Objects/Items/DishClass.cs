using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName ="new Dish Class", menuName = "Item/Dish")]
public class DishClass : Item
{
    public int hunger;

    public override void UseItem(SCPlayerMovement caller){
        if(caller.hunger + hunger <= caller.maxHunger){
            caller.hunger += hunger;
        }
    }

    public override DishClass GetDish() { return this; }
}

