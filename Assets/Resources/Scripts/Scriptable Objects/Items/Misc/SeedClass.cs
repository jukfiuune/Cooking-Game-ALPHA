using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Seed", menuName = "Item/Misc/Seed")]
public class SeedClass : MiscClass
{
    public PlantBase Plant;
    
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
        if(hit.collider != null){
            if(hit.collider.tag == "PlantSpot"){
                if(hit.collider.GetComponent<PlantSpotScript>().Plant == null){
                    Vector3 interactDistance = new Vector3(position.x, position.y, 0);
                    if (Vector3.Distance(interactDistance, caller.transform.position) <= 1)
                    {
                        caller.MouseInventory.RemoveAt(0, 1);
                        hit.collider.GetComponent<PlantSpotScript>().Plant = Plant;
                    }
                    else
                    {
                        caller.PointToMoveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        caller.ShouldMoveToPoint = true;
                        caller.moveToPointAction = "UseHand";
                        caller.moveToRange = 1;
                    }      
                }
            }else{
                caller.DropItemFromPlayer();
            }
        }else{
            caller.DropItemFromPlayer();
        }
    }
}
