using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemPickup : MonoBehaviour
{
    public Inventory inv;
    private void Start()
    {
        inv = GetComponent<Inventory>();    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "item")
        {
            itemPickup iP = collision.GetComponent<itemPickup>();
            int leftover = inv.AddItem(iP.item, iP.amount);
            if (leftover > 0)
            {
                iP.amount = leftover;
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
