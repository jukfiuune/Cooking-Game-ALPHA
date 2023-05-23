using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Hammer", menuName = "Item/Tool/Hammer")]
public class HammerClass : ToolClass
{   
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
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity);
        //Check if ray hit a farm plot
        if (hit.collider != null)
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("DestructableStructure"))
            {
                //Distance       
                Vector3 interactDistance = new Vector3(position.x, position.y, 0);
                if (Vector3.Distance(interactDistance, caller.transform.position) <= range)
                {
                    durability -= 20;
                    base.UseItem(caller);
                    Destroy(hit.transform.gameObject);
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
