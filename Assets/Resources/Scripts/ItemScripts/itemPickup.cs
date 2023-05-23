using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class itemPickup : MonoBehaviour
{
    public Item item;
    public int amount;
    TextMeshProUGUI amountText;
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemIcon;
    }

}
