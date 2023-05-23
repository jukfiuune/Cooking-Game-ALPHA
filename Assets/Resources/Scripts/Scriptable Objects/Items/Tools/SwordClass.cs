using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Sword", menuName = "Item/Tool/Sword")]
public class SwordClass : ToolClass
{
    public LayerMask EnemyLayer;

    public override void UseItem(SCPlayerMovement caller){
       
        //if(CanSwing()){
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if(hit.collider != null){
            if(hit.collider.GetComponent<InventoryInteract>() == null){
                Swing(caller);
            }
        }else{
            Swing(caller);
        }
        //}
    }

    public void Swing(SCPlayerMovement caller){
        //Debug.Log("swing");
        
        float dist = 2f;

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - caller.transform.position;
        float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;

        float xDist = dist * Mathf.Cos(angle * Mathf.Deg2Rad);
        float yDist = dist * Mathf.Sin(angle * Mathf.Deg2Rad);

        //GameObject attack = Instantiate(square, new Vector3(caller.transform.position.x + xDist, caller.transform.position.y + yDist, 0), Quaternion.identity);
       
        //attack.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Collider2D[] enemiesHit = Physics2D.OverlapBoxAll(new Vector2(caller.transform.position.x + xDist, caller.transform.position.y + yDist), new Vector2(1, 2), angle, EnemyLayer); 

        if(enemiesHit[0] != null){
            durability -= 15;
            base.UseItem(caller);
        }

        foreach (var enemy in enemiesHit)
        {
            //Destroy(enemy.gameObject);
            enemy.GetComponent<EnemyScript>().Hit(damage);
        }
    }
}
