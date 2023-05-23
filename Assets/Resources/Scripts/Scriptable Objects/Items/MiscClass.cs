using System.Collections;
using UnityEngine;
[CreateAssetMenu(fileName = "new Misc Class", menuName = "Item/Misc/Misc")]
public class MiscClass : Item
{
    public override void UseItem(SCPlayerMovement caller) { }

    public override MiscClass GetMisc() { return this; }
}
