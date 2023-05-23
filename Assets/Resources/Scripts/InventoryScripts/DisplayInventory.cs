using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public List<GameObject> slots = new List<GameObject>(); // The list of slots this script interacts with
    public Inventory TargetInventory;  // the inventory of the object that called this script / created the object with this script
    public Inventory MouseInventory;   // The mouse/hand inventory
    public Inventory PlayerInventory;  // The player inventory
    public GameObject mouseIcon;       // The image object that follow the mouse
    public bool mICheck;               // Should this script change mouseIcons image
    public TextMeshProUGUI mouseAmount;// The text script attached to a gameobject that follows the mouse

    void Start()
    {  
        // Find the player inventory
        PlayerInventory = GameObject.Find("Body").GetComponent<Inventory>();
        if (mICheck)
        {
            // This checks if this script is responsible for the displaying of mouseInventory and if so does some "special things"
            TargetInventory = GameObject.Find("Body").GetComponent<Inventory>();
            MouseInventory = GetComponent<Inventory>();
            mouseAmount = mouseIcon.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        else
        {
            // Finds the mouse/hand inventory
            MouseInventory = GameObject.Find("PlayerInventory").GetComponent<Inventory>();
        }
            
        for (int i = 0; i < slots.Count; i++)
        {
            // Sets the slot variable of all the slots in the slots list
            slots[i].GetComponent<SlotInteract>().slot = i;
        }
        UpdateSlots();
    }

    void Update()
    {   
        if (mICheck)
        {
            // Move the mouse/hand icon to the mouse and set it infront of everything
            mouseIcon.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            transform.SetAsLastSibling();
        }

        if (TargetInventory.displayUpdate)
        {
            // If there is an update in TargetInventorys inventory then loop through all slots and change their icons and amounts
            TargetInventory.displayUpdate = false;
            UpdateSlots();
            // ^
        }

        if (mICheck && MouseInventory.displayUpdate)
        {
            // Change the mouse icon and amount
            Item it = MouseInventory.items[0];
            if (it != null)
            {
                mouseIcon.GetComponent<Image>().sprite = it.itemIcon;
                mouseIcon.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                if(MouseInventory.items[0].GetTool() != null){
                    mouseAmount.text = MouseInventory.items[0].GetTool().durability.ToString() + "%";
                }else if(MouseInventory.items[0].GetArmor() != null){
                    mouseAmount.text = MouseInventory.items[0].GetArmor().durability.ToString() + "%";
                }else{
                    mouseAmount.text = MouseInventory.amounts[0].ToString();
                }
            }
            else
            {
                mouseIcon.GetComponent<Image>().sprite = null;
                mouseIcon.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                mouseAmount.text = "";
            }
        }
    }

    void UpdateSlots()
    {
        for (int i = 0; i < TargetInventory.inventorySlots; i++)
        {
            GameObject sl = slots[i];
            Image im = sl.transform.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI tx = sl.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            if (TargetInventory.items[i] != null)
            {
                im.sprite = TargetInventory.items[i].itemIcon;
                im.color = new Color(1, 1, 1, 1);
            }
            else
            {
                im.sprite = null;
                im.color = new Color(1, 1, 1, 0);
            }

            int amount = TargetInventory.amounts[i];
            string displayAmount;
            if (amount != 0)
            {
                if (TargetInventory.items[i].GetTool() != null)
                {
                    displayAmount = TargetInventory.items[i].GetTool().durability.ToString() + "%";

                }
                else if (TargetInventory.items[i].GetArmor() != null)
                {
                    displayAmount = TargetInventory.items[i].GetArmor().durability.ToString() + "%";

                }
                else
                {
                    displayAmount = amount.ToString();
                }
                tx.text = displayAmount;
            }
            else
            {
                tx.text = "";
            }
        }
    }
}
