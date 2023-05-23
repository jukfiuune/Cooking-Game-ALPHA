using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile_collision : MonoBehaviour
{


    public Damage d;
 string hitthing = "Player";
string wall = "wall";

void Start()
    {

        d = GameObject.Find("Healthbar").GetComponent<Damage>();
    }


 void OnTriggerEnter2D(Collider2D col){

if(col.gameObject.tag == hitthing){

 d.DealDamage(5);
                
         Object.Destroy(this.gameObject);


}


if(col.gameObject.tag == wall){
                
         Object.Destroy(this.gameObject);


}





 }
}
