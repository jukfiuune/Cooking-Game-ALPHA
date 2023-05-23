using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;




public class enemyfollow : MonoBehaviour
{

private Damage d;
Rigidbody2D rb;
[SerializeField] Transform target;
NavMeshAgent agent;
private GameObject objectToFind;
string tagName = "Player";
private Transform EnemyTarget;
private bool Offence = true;
private int cooldown=200;



void Start(){




d = GameObject.Find("Healthbar").GetComponent<Damage>();

objectToFind = GameObject.FindGameObjectWithTag(tagName);
EnemyTarget=objectToFind.GetComponent<Transform>();


//GetChild(0)

agent = GetComponent<NavMeshAgent>();
agent.updateRotation=false;
agent.updateUpAxis = false;





}



void Update(){



if(Offence==true){

agent.SetDestination(EnemyTarget.position);

}else{

agent.SetDestination(EnemyTarget.position*-10);
cooldown--;
}


if(cooldown<0){

Offence=true;
cooldown=200;


}



}




 void OnCollisionEnter2D(Collision2D collision)
    {
         
         
          if (collision.gameObject.tag == "Player")
        {

d.DealDamage(5);
  Offence=false;    
      
        }
         
          

    }

   
    






}
