using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    [Header("Item")]
    public string Name;
    public string Description;
    public int stackSize;
    public Sprite itemIcon;
    public string filterType; // Tool, Food, Helmet, Misc etc...

    public virtual void UseItem(SCPlayerMovement caller)
    {
        Debug.Log("Used item");
    }

    public virtual void DropItem(Vector3 position, Item itemToDrop, int amountToDrop){
        GameObject itemPickup = Instantiate(Resources.Load<GameObject>("Prefabs/Inventory/ItemPickup"), position, Quaternion.identity);
        itemPickup.GetComponent<itemPickup>().item = itemToDrop;
        itemPickup.GetComponent<itemPickup>().amount = amountToDrop;
    }

    public virtual Item GetItem() {return this;}
    public virtual FoodClass GetFood() {return null;}
    public virtual MiscClass GetMisc() {return null;}
    public virtual ToolClass GetTool() {return null;}
    public virtual ArmorClass GetArmor() {return null;}
    public virtual DishClass GetDish() {return null;}
    public virtual BuildingClass GetBuilding() {return null;}

}
