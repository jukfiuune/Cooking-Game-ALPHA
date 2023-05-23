using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    // Start is called before the first frame update
    // Use this for initialization
    void Awake () 
    {
        Cursor.visible = false;
    }
   
    // Update is called once per frame
    void Update () 
    {
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousepos;
    }
}
