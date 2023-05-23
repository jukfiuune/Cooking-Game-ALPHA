using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObjectScript : MonoBehaviour
{
    public int hitsToTake = 3;

    public Item itemToDrop;

    public int amountToDrop = 3;

    public GameObject tempPrefab;

    public string destroyedBy;

    private void Start() {
        tempPrefab = Resources.Load<GameObject>("Prefabs/Inventory/ItemPickup");
        // Sort By Y
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * -32);
    }
    
    public void Clicked(){
        hitsToTake--;
        
        if(hitsToTake == 0){
            itemPickup item = Instantiate(tempPrefab, transform.position, Quaternion.identity).GetComponent<itemPickup>();
            item.item = itemToDrop;
            item.amount = amountToDrop;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collison) {
        if(collison.gameObject.GetComponent<DestructableObjectScript>() != null){
            Destroy(gameObject);
        }
    }
}
