using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName ="new Food Class", menuName = "Item/Food")]
public class FoodClass : Item
{
    public string type;
    public int points;
    public int hunger;

    public override void UseItem(SCPlayerMovement caller){
        if(caller.hunger + hunger <= caller.maxHunger){
            caller.hunger += hunger;
        }
    }

    public override FoodClass GetFood() { return this; }

}

