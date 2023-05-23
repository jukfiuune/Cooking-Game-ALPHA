using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Tool/Tool")]
public class ToolClass : Item
{
    [Header("Tool")]
    
    public toolType type;
    public enum toolType
    {
        Axe,
        Pickaxe,
        Sword,
        Shovel,
        Torch,
        Other
    };
    public int durability;
    public float damage;
    public float range;
    public float speed;
    float nextSwing = 0f;
    //private bool canSwing = true;

    public override void UseItem(SCPlayerMovement caller)
    {
        //base.UseItem(caller);
        //Debug.Log("Swing tool");
        
        caller.playerInventory.InventoryUpdate();

        if(durability <= 0){
            Destroy(this);
            caller.playerInventory.RemoveAt(9, 1);
        }
        
    }

    public bool CanSwing(){
        //Debug.Log("check");
        if(Time.time >= nextSwing){
            nextSwing = Time.time + 1f / speed;
            return true;
        }else{
            return false;
        }
    }
    
    public override ToolClass GetTool() { return this; }

}
