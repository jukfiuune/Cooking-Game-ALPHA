using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourSideenemy : MonoBehaviour
{
public float Speed= 0.01f;
public GameObject Projectile;
public Transform Source;
private Rigidbody2D rb2D;
private Transform EnemyTarget;
private GameObject objectToFind;



string tagName="Player";

int update=0;


    // Update is called once per frame
    void Update()
    {

update++;

objectToFind = GameObject.FindGameObjectWithTag(tagName);
EnemyTarget=objectToFind.GetComponent<Transform>();
transform.position = Vector2.MoveTowards(transform.position, EnemyTarget.position, Speed);

if(update%50==0){

GameObject patron;

patron = Instantiate(Projectile,Source.position,Source.rotation);
patron.GetComponent<Rigidbody2D>().velocity = new Vector2(5f,0f);

patron = Instantiate(Projectile,Source.position,Source.rotation);
patron.GetComponent<Rigidbody2D>().velocity = new Vector2(-5f,0f);


patron = Instantiate(Projectile,Source.position,Source.rotation);
patron.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,-5f);

patron = Instantiate(Projectile,Source.position,Source.rotation);
patron.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,5f);


}
    


    }
}
