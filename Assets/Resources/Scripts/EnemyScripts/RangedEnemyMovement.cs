using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangedEnemyMovement : MonoBehaviour
{

public float Speed= 0.01f;
public GameObject Projectile;
public Transform Source;
private Rigidbody2D rb2D;
private Transform EnemyTarget;
private GameObject objectToFind;
public int cooldown = 40;

string tagName="Player";

int update=0;


    // Update is called once per frame
    void Update()
    {

update++;

objectToFind = GameObject.FindGameObjectWithTag(tagName);
EnemyTarget=objectToFind.GetComponent<Transform>();
transform.position = Vector2.MoveTowards(transform.position, EnemyTarget.position, Speed);

if(update%cooldown==0){

GameObject patron;

patron = Instantiate(Projectile,Source.position,Source.rotation);



Vector2 fromEnemyToPlayer= Source.position-EnemyTarget.position;
//fromEnemyToPlayer.Normalize();
Vector2 velocity = fromEnemyToPlayer * 1f;

patron.GetComponent<Rigidbody2D>().velocity = -1*velocity;

}
    


    }


}
