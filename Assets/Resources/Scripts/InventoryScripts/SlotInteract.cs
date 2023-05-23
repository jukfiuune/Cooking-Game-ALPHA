using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotInteract : MonoBehaviour
{
    DisplayInventory displayInventory;       // The DisplayInventory of the parent object of the slot
    public int slot;           // The index of the slot in TargetInventory of displayInventory which this gameobject represents
    OpenedUI UIOpen;           // Check this script for info
    Inventory playerInventory; // The player inventory
    SCPlayerMovement playerController;

    void Start()
    {
        displayInventory = transform.parent.GetComponent<DisplayInventory>();
        UIOpen = GameObject.Find("UIOpen").GetComponent<OpenedUI>();
        playerInventory = GameObject.Find("Body").GetComponent<Inventory>();
        playerController = playerInventory.transform.GetComponent<SCPlayerMovement>();
    }

    public void onClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //lclick
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //shift
                // Quick move (lshift + lclick)
                QuickMove();
            }
            else
            {
                //
                if (displayInventory.TargetInventory.items[slot] == null || displayInventory.MouseInventory.items[0] == null || displayInventory.TargetInventory.items[slot].Name != displayInventory.MouseInventory.items[0].Name)
                {
                    // Swap (lclick)
                    Swap();
                }
                else
                {
                    // Combine stack (lclick)
                    CombineStack();
                    Debug.Log("CombineStack");
                }
            }
        }
        else
        {
            //rclick
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //shift
                if (displayInventory.MouseInventory.amounts[0] == 0 && displayInventory.TargetInventory.amounts[slot] != 1)
                {
                    // Split (lshift + rclick + empty hand)
                    Split();
                }
                //
                else if (displayInventory.MouseInventory.items[0] != null)
                {
                    // Add one (lshift + rclick + full hand)
                    AddOne();

                }
            }
            else
            {
                // Right click to equip
                RightClickEquip();
            }
        }
    }
    void QuickMove()
    {
        // Quick move (lshift + lclick)
        if (UIOpen.openedInventory != null)
        {
            //i want to cry : ^)
                        //-nio8 <3
            if(UIOpen.openedInventory.transform.tag == "CookingStation"){
                if (UIOpen.openedInventory != displayInventory.TargetInventory)
                {
                    //allows amount of 1 in each slot if we are interacting with a cooking station
                    for(int i = 0; i < UIOpen.openedInventory.inventorySlots; i++){
                        if(UIOpen.openedInventory.items[i] == null){
                            int leftover = UIOpen.openedInventory.AddItemAt(displayInventory.TargetInventory.items[slot], 1, i);
                            displayInventory.TargetInventory.RemoveAt(slot, 1 - leftover);
                            break;
                        }
                    }
                }
                else
                {
                    int leftover = displayInventory.PlayerInventory.AddItem(UIOpen.openedInventory.items[slot], UIOpen.openedInventory.amounts[slot]);
                    UIOpen.openedInventory.RemoveAt(slot, UIOpen.openedInventory.amounts[slot]-leftover);
                }
            }else{
                if (UIOpen.openedInventory != displayInventory.TargetInventory)
                {
                    //from displayInventory.TargetInventory (player inventory) to UI inventory  
                    int leftover = UIOpen.openedInventory.AddItem(displayInventory.TargetInventory.items[slot], displayInventory.TargetInventory.amounts[slot]);
                    displayInventory.TargetInventory.RemoveAt(slot, displayInventory.TargetInventory.amounts[slot] - leftover);
                }
                else
                {
                    int leftover = displayInventory.PlayerInventory.AddItem(UIOpen.openedInventory.items[slot], UIOpen.openedInventory.amounts[slot]);
                    UIOpen.openedInventory.RemoveAt(slot, UIOpen.openedInventory.amounts[slot]-leftover);
                }
            }
        }
    }
    void CombineStack()
    {
        // Combine stack (lclick)
        Item it = displayInventory.TargetInventory.items[slot];
        if (it != null)
        {
            if(UIOpen.openedInventory != null && UIOpen.openedInventory.transform.tag == "CookingStation" && displayInventory.TargetInventory == UIOpen.openedInventory){
               //you can only combine stacks if you are outside of a cooking station
            }else{
                 if (displayInventory.TargetInventory.amounts[slot] < it.stackSize && displayInventory.MouseInventory.amounts[0] < it.stackSize)
                {
                    int overflow = displayInventory.TargetInventory.AddItemAt(it, displayInventory.MouseInventory.amounts[0], slot);

                    if (overflow == 0)
                    {
                        displayInventory.MouseInventory.items[0] = null;
                        displayInventory.MouseInventory.amounts[0] = 0;
                    }
                    else
                    {
                        displayInventory.MouseInventory.amounts[0] = overflow;
                    }
                }
                else
                {
                    displayInventory.TargetInventory.SwapInventory(slot, displayInventory.MouseInventory, 0);
                }
            }
        }
    }
    void Swap()
    {
        // Swap (lclick)
        if(UIOpen.openedInventory != null && displayInventory.MouseInventory.items[0] != null && displayInventory.TargetInventory == UIOpen.openedInventory){
            //disables swapping in cooking stations if the swap results in a slot with amount of more than 1
            if(UIOpen.openedInventory.transform.tag == "CookingStation"){
                if(UIOpen.openedInventory.amounts[slot] == 0){
                    int leftover = UIOpen.openedInventory.AddItemAt(displayInventory.MouseInventory.items[0], 1, slot);
                    displayInventory.MouseInventory.RemoveAt(0, 1 - leftover);
                }
            }else{
                displayInventory.TargetInventory.SwapInventory(slot, displayInventory.MouseInventory, 0);
            }
        }else{
            displayInventory.TargetInventory.SwapInventory(slot, displayInventory.MouseInventory, 0);
        }
    }
    void Split()
    {
        // Split (lshift + rclick + empty hand)
        Item it = displayInventory.TargetInventory.items[slot];
        if (it != null)
        {
            float halfAmount = (float)displayInventory.TargetInventory.amounts[slot] / 2;
            displayInventory.TargetInventory.RemoveAt(slot, displayInventory.TargetInventory.amounts[slot] - Mathf.FloorToInt(halfAmount));
            displayInventory.MouseInventory.AddItem(displayInventory.TargetInventory.items[slot], Mathf.CeilToInt(halfAmount));
        }
    }
    void AddOne()
    {
        // Add one (lshift + rclick + full hand)
        Item itIf = displayInventory.TargetInventory.items[slot];
        if(UIOpen.openedInventory != null && UIOpen.openedInventory.transform.tag == "CookingStation" && displayInventory.TargetInventory == UIOpen.openedInventory && UIOpen.openedInventory.amounts[slot] != 0){ 
            //special case if we are interacting with a cooking station (tbh no idea what the problem was but it's fixed now)
        }else{
            if (itIf == null || (displayInventory.MouseInventory.items[0].Name == itIf.Name && displayInventory.TargetInventory.amounts[slot] < itIf.stackSize))
            {
                Item it = displayInventory.MouseInventory.items[0];
                if (displayInventory.TargetInventory.limits[slot] == "" || displayInventory.TargetInventory.limits[slot] == null || displayInventory.TargetInventory.limits[slot] == it.filterType)
                {
                    Debug.Log(displayInventory.TargetInventory.AddItemAt(it, 1, slot));
                    displayInventory.MouseInventory.amounts[0]--;
                    if (displayInventory.MouseInventory.amounts[0] == 0)
                    {
                        displayInventory.MouseInventory.items[0] = null;
                    }
                }
            }
        }
    }
    void RightClickEquip()
    {
        // Right click to equip
        if (displayInventory.TargetInventory.amounts[slot] > 0)
        {
            Item it = displayInventory.TargetInventory.items[slot];
            if (displayInventory.TargetInventory.gameObject.name == "Body" && slot > 8)
            {
                int emptySlot = playerInventory.GetEmptySlot(playerInventory.items[slot].filterType);
                if (emptySlot != -1)
                {
                    playerInventory.SwapInventory(emptySlot, playerInventory, slot);
                     
                }
            }
            else if (it.filterType == "Tool")
            {
                displayInventory.TargetInventory.SwapInventory(slot, playerInventory, 9);
            }
            else if (it.filterType == "Chestplate")
            {
                displayInventory.TargetInventory.SwapInventory(slot, playerInventory, 10);
            }
            else if (it.filterType == "Helmet")
            {
                displayInventory.TargetInventory.SwapInventory(slot, playerInventory, 11);
            }else if(displayInventory.TargetInventory.items[slot].GetFood() != null){
                if(playerController.hunger + displayInventory.TargetInventory.items[slot].GetFood().hunger <= playerController.maxHunger){
                    displayInventory.TargetInventory.items[slot].UseItem(playerController);
                    displayInventory.TargetInventory.RemoveAt(slot, 1);
                }
            }else if(displayInventory.TargetInventory.items[slot].GetDish() != null){
               if(playerController.hunger + displayInventory.TargetInventory.items[slot].GetDish().hunger <= playerController.maxHunger){
                    displayInventory.TargetInventory.items[slot].UseItem(playerController);
                    displayInventory.TargetInventory.RemoveAt(slot, 1);
                } 
            }
        }
    }
}
