using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Fertilizer", menuName = "Item/Misc/Fertilizer")]
public class FertilizerClass : MiscClass
{
    public int fertilizerPoints;
    public LayerMask FarmPlotLayer;

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
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, FarmPlotLayer);

        if (hit.collider != null){
            FarmPlotScript FarmPlot = hit.collider.GetComponent<FarmPlotScript>();

            if(FarmPlot != null){
                Vector3 interactDistance = new Vector3(position.x, position.y, 0);
                if (Vector3.Distance(interactDistance, caller.transform.position) <= 1)
                {
                    caller.MouseInventory.RemoveAt(0, 1);
                    FarmPlot.GiveNutrients(0, fertilizerPoints);
                }
                else
                {
                    caller.PointToMoveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    caller.ShouldMoveToPoint = true;
                    caller.moveToPointAction = "UseHand";
                    caller.moveToRange = 1;
                }
            }else{
                caller.DropItemFromPlayer();
            }
        }else{
            caller.DropItemFromPlayer();
        }
    }
}
