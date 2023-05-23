using System.Collections;
using UnityEngine;
[CreateAssetMenu(fileName = "new Building Class", menuName = "Item/Misc/Building")]
public class BuildingClass : MiscClass
{
    public GameObject building;
    public override void UseItem(SCPlayerMovement caller){ }

    public override BuildingClass GetBuilding() { return this; }
}
