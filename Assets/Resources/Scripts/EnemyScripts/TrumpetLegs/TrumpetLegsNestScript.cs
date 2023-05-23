using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetLegsNestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TrumpetLegsScript trumpetLegs = Instantiate(Resources.Load<GameObject>("Prefabs/TrumpetLegs"), transform.position, Quaternion.identity).GetComponent<TrumpetLegsScript>();
        trumpetLegs.Nest = gameObject;
    }

}
