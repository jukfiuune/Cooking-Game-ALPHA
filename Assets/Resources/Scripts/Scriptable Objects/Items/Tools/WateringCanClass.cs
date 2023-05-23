using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Watering Can", menuName = "Item/Tool/Watering Can")]
public class WateringCanClass : ToolClass
{   
    public override void UseItem(SCPlayerMovement caller){
        Vector3 position;
        if (caller.ItemUseMousePositionInstead)
        {
            position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            position = caller.PointToMoveTo;
        }
        
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

        if (hit.collider != null)
        {

            FarmPlotScript FarmPlot = hit.collider.GetComponent<FarmPlotScript>();
            if (hit.collider.tag == "Water Body" || FarmPlot != null)
            {
                Vector3 interactDistance = new Vector3(position.x, position.y, 0);

                if (Vector3.Distance(interactDistance, caller.transform.position) <= range)
                {
                    if (hit.collider.tag == "Water Body")
                    {
                        durability = 100;
                        caller.playerInventory.InventoryUpdate();
                    }
                    else if (durability >= 10)
                    {
                        FarmPlot.GiveNutrients(1, 0);
                        durability -= 10;
                        caller.playerInventory.InventoryUpdate();
                    }
                }
                else
                {
                    caller.PointToMoveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    caller.ShouldMoveToPoint = true;
                    caller.moveToPointAction = "UseItem";
                    caller.moveToRange = range;
                }
            }
        }
    }
}

