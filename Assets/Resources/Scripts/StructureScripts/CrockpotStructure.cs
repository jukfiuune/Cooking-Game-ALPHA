using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrockpotStructure : MonoBehaviour
{
    public bool startedCooking = false;
    public float cookTimer;
    public Item product;
    bool ready = false;
    Inventory playerInventory;

    GameObject flame;
    GameObject back;
    GameObject indicator;
    GameObject lid;
    InventoryInteract inventoryInteract;
    void Start()
    {
        playerInventory = GameObject.Find("Body").GetComponent<Inventory>();
        inventoryInteract = transform.GetComponent<InventoryInteract>();
        // Decor
        flame     = transform.GetChild(0).gameObject;
        indicator = transform.GetChild(1).gameObject;
        back      = transform.GetChild(2).gameObject;
        lid       = transform.GetChild(3).gameObject;
    }

    void Update()
    {
        if (startedCooking)
        {
            cookTimer -= Time.deltaTime;
            if (cookTimer <= 0)
            {
                ready = true;
                startedCooking = false;

                // Decor
                flame.SetActive(false);
                back.SetActive(true);
                indicator.SetActive(true);
                lid.SetActive(false);

                indicator.GetComponent<SpriteRenderer>().sprite = product.itemIcon;
            }
        }

        if(inventoryInteract.opened || indicator.activeSelf){
            back.SetActive(true);
            lid.SetActive(false);
        }else{
            back.SetActive(false);
            lid.SetActive(true);
        }
    }
    private void OnMouseDown()
    {
        if (ready)
        {
            float dist = Vector3.Distance(transform.position, playerInventory.transform.position);
            if (dist <= inventoryInteract.openDist)
            {
                playerInventory.AddItem(product, 1);
                inventoryInteract.interactable = true;

                ready = false;

                // Decor
                flame.SetActive(false);
                back.SetActive(false);
                indicator.SetActive(false);
                lid.SetActive(true);
            }
        }
    }
    public void StartCooking(Item _product, float _cookTimer) 
    {
        flame.SetActive(true);
        inventoryInteract.interactable = false;
        cookTimer = _cookTimer;
        product = _product;
        startedCooking = true;
    }
}
