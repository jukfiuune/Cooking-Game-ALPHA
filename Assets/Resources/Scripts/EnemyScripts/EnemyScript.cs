using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyScript : MonoBehaviour
{
    public float speed;
    public float health;
    public float damage;

    public TMP_Text healthText;

    private void Start() {
        healthText.text = health.ToString();
    }

    public void Hit(float damageTaken){
        Knockback();
        health -= damageTaken;
        healthText.text = health.ToString();
        IsDead();
    }

    private void IsDead(){
        if(health <= 0){
            Destroy(gameObject);
        }
    }

    public void Knockback(){
        GetComponent<Rigidbody2D>().AddForce(-(GameObject.Find("Body").transform.position - transform.position) * 50);
    }
}
