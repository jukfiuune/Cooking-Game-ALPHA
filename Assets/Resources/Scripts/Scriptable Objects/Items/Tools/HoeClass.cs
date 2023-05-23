using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Hoe", menuName = "Item/Tool/Hoe")]
public class HoeClass : ToolClass
{
    public LayerMask PlantSpotLayer;
    public LayerMask FarmPlotLayer;
    
    public override void UseItem(SCPlayerMovement caller)
    {
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
        //Check if ray hit a farm plot
        if (hit.collider != null)
        {
            FarmPlotScript FarmPlot = hit.collider.GetComponent<FarmPlotScript>();
            if (FarmPlot != null)
            {
                //Distance       
                Vector3 interactDistance = new Vector3(position.x, position.y, 0);
                if (Vector3.Distance(interactDistance, caller.transform.position) <= range)
                {
                    // Check for plant spots
                    Collider2D hitPlantSpot = Physics2D.OverlapArea(new Vector2(position.x - 0.6f,
                                                                                position.y + 0.4f),
                                                                    new Vector2(position.x + 0.6f,
                                                                                position.y - 0.4f),
                                                                                PlantSpotLayer);
                    if (hitPlantSpot == null)
                    {
                        GameObject PlantSpot = Instantiate(Resources.Load<GameObject>("Prefabs/Farm/PlantSpot"), position, Quaternion.identity);
                        PlantSpot.GetComponent<PlantSpotScript>().myFarmPlot = FarmPlot;
                        durability -= 5;
                        base.UseItem(caller);

                        FarmPlot.soilList.Add(PlantSpot);
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