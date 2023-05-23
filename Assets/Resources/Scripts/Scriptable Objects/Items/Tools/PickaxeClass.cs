using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Pickaxe", menuName = "Item/Tool/Pickaxe")]
public class PickaxeClass : ToolClass
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
        
        if(hit.collider != null){
            DestructableObjectScript objectHit = hit.collider.GetComponent<DestructableObjectScript>();
        
            if(objectHit != null){
                if (type.ToString() == objectHit.destroyedBy){
                    Vector3 interactDistance = new Vector3(position.x, position.y, 0);
                    if (Vector3.Distance(interactDistance, caller.transform.position) <= range)
                    {
                        objectHit.Clicked();
                        durability -= 5;
                        base.UseItem(caller);
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
}    