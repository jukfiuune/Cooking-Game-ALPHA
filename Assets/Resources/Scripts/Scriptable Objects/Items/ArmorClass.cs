using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new Armor Class", menuName = "Item/Armor")]
public class ArmorClass : Item
{
    public armorType type;
    public enum armorType
    {
        helmet,
        boobplat
    };
    public int durability;
    public float protection;

    public override ArmorClass GetArmor() { return this; }

}
