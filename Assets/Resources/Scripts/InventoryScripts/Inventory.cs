using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int inventorySlots;
    public Item[] items;
    public int[] amounts;
    public string[] limits;

    public bool displayUpdate;
    private void Start()
    {
        items = new Item[inventorySlots];
        amounts = new int[inventorySlots];
        if (limits.Length != inventorySlots)
        {
            limits = new string[inventorySlots];
        }
        for (int i = 0; i < inventorySlots; i++)
        {
            items[i] = null;
            amounts[i] = 0;
        }
        InventoryUpdate();
    }
    public int AddItem(Item _itemBase, int _amount)
    {
        if (_amount <= 0)
        {
            return _amount;
        }
        int foundSlots = 0;
        int canFit = 0;
        int cantFit = 0;
        //Proverq kolko slota sa nujni za _amount itema
        float neededSlots = Mathf.Ceil((float)_amount / _itemBase.stackSize);
        for (int i = 0; i < inventorySlots; i++)
        {
            if (items[i] != null && items[i].Name == _itemBase.Name && amounts[i] < _itemBase.stackSize)
            {
                // Proverqva dali mojesh da poberesh itemite v edin slot po-malko
                neededSlots--;
                foundSlots++;
                canFit += _itemBase.stackSize - amounts[i];
            }
            else if (items[i] == null && (limits[i] == null || limits[i] == "" || limits[i] == _itemBase.filterType))
            {
                canFit += _itemBase.stackSize;
            }
        }
        //Proverqva dali ima dostatychno mqsto v inventara
        if (canFit < _amount)
        {
            cantFit = _amount - canFit;
            _amount = canFit;
        }
        //Dobavqne na itemi
        while (neededSlots + foundSlots > 0)
        {
            bool found = false;
            int place;
            if (foundSlots > 0)
            {
                for (place = 0; place < inventorySlots; place++)
                {
                    if (items[place] != null && items[place].Name == _itemBase.Name && amounts[place] != _itemBase.stackSize && (limits[place] == null || limits[place] == "" || limits[place] == _itemBase.filterType))
                    {
                        if (amounts[place] + _amount <= _itemBase.stackSize)
                        {
                            amounts[place] += _amount;
                            found = true;
                            break;
                        }
                        else
                        {
                            _amount -= _itemBase.stackSize - amounts[place];
                            amounts[place] = _itemBase.stackSize;
                        }
                        foundSlots--;
                    }
                }
            }
            if (!found)
            {
                place = GetEmptySlot(_itemBase.filterType);
                Item _item = Instantiate(_itemBase);
                _item.name = _itemBase.name;
                if (place != -1 && (limits[place] == null || limits[place] == "" || limits[place] == _item.filterType))
                {
                    items[place] = _item;
                    if (_amount > _item.stackSize)
                    {
                        _amount -= _item.stackSize;
                        amounts[place] = _item.stackSize;
                    }
                    else
                    {
                        amounts[place] = _amount;
                        _amount = 0;
                    }
                }
                else
                {
                    cantFit += _amount;
                    _amount = 0;
                }
            }
            if (neededSlots != 1 || _amount == 0)
            {
                neededSlots--;
            }
        }
        InventoryUpdate();
        return cantFit;
    }
    public int AddItemAt(Item _item, int _amount, int _slot)
    {
        if (_amount <= 0 || (limits[_slot] != _item.filterType && limits[_slot] != "" && limits[_slot] != null))
        {
            return _amount;
        }
        if (items[_slot] == null )
        {
            items[_slot] = _item;
            amounts[_slot] = _amount;
            InventoryUpdate();
            return 0;
        }
        if (items[_slot] == null || (items[_slot].Name != _item.Name))
        {
            return _amount;
        }
        if (amounts[_slot] + _amount < _item.stackSize) {
            amounts[_slot] += _amount;
            InventoryUpdate();
            return 0;
        }
        else if (GetEmptySlotNumber() > 0)
        {
            _amount -= _item.stackSize - amounts[_slot];
            amounts[_slot] = _item.stackSize;
            InventoryUpdate();
            return _amount;
        }

        return _amount;
    }
    public int RemoveAt(int _slot, int _amount)
    {
        // Removed "_amount" amount of items from "_slot" slot
        int overflow = 0;
        if (_amount <= amounts[_slot])
        {
            amounts[_slot] -= _amount;
            if (amounts[_slot] == 0)
            {
                items[_slot] = null;
            }
        }
        else
        {
            overflow = _amount - amounts[_slot];
            amounts[_slot] = 0;
            items[_slot] = null;
        }
        InventoryUpdate();
        return overflow;
    }
    public bool RemoveItem(Item _item, int _amount)
    {
        int itemAm = GetItemAmount(_item);

        if (itemAm >= _amount)
        {
            for (int i = inventorySlots - 1; i >= 0; i--)
            {
                if (items[i] != null && items[i].Name == _item.Name)
                {
                    if (_amount > amounts[i])
                    {
                        _amount -= amounts[i];
                        amounts[i] = 0;
                        items[i] = null;
                    }
                    else
                    {
                        amounts[i] -= _amount;
                        if (amounts[i] == 0)
                        {
                            items[i] = null;
                        }
                        InventoryUpdate();
                        return true;
                    }
                }
            }
        }

        return false;
    }
    public void SwapItems(int slot1, int slot2)
    {
        Item it1 = items[slot1];
        Item it2 = items[slot2];
        string it1Limit;
        string it2Limit;
        if (it1 != null)
        {
            it1Limit = it1.filterType;
        }
        else
        {
            it1Limit = "";
        }
        if (it2 != null)
        {
            it2Limit = it2.filterType;
        }
        else
        {
            it2Limit = "";
        }
        if ((limits[slot1] == null || limits[slot1] == "" || it2 == null || limits[slot1] == it2Limit) && (limits[slot2] == null || limits[slot2] == "" || it1 == null || limits[slot2] == it1Limit))
        {
            int am = amounts[slot1];
            items[slot1] = it2;
            amounts[slot1] = amounts[slot2];
            items[slot2] = it1;
            amounts[slot2] = am;
            InventoryUpdate();
        }

    }
    public void SwapInventory(int _slot, Inventory _inventory, int _otherSlot)
    {
        Item it1 = items[_slot];
        Item it2 = _inventory.items[_otherSlot];
        string it1Limit;
        string it2Limit;
        if (it1 != null)
        {
            it1Limit = it1.filterType;
        }
        else
        {
            it1Limit = "";
        }
        if (it2 != null)
        {
            it2Limit = it2.filterType;
        }
        else
        {
            it2Limit = "";
        }      
        if ((limits[_slot] == null || limits[_slot] == "" || it2 == null || limits[_slot] == it2Limit) && (_inventory.limits[_otherSlot] == null || _inventory.limits[_otherSlot] == "" || it1 == null || _inventory.limits[_otherSlot] == it1Limit))
        {
            int am1 = amounts[_slot];
            items[_slot] = it2;
            amounts[_slot] = _inventory.amounts[_otherSlot];
            _inventory.items[_otherSlot] = it1;
            _inventory.amounts[_otherSlot] = am1;
            InventoryUpdate();
            _inventory.InventoryUpdate();
        }

    }
    public int GetItemAmount(Item _item)
    {
        int itemAm = 0;
        for (int i = 0; i < inventorySlots; i++)
        {
            if (items[i] != null && items[i].Name == _item.Name)
            {
                itemAm += amounts[i];
            }
        }
        return itemAm;
    }
    public int GetItemPlace(Item _item)
    {
        // returns -1 if there is no slots with _item in them
        // otherwise returns the index of the first slot with _item
        for (int i = 0; i < inventorySlots; i++)
        {
            if (items[i].Name == _item.Name)
            {
                return i;
            }
        }
        return -1;
    }
    public int GetItemFitAmount(Item _item)
    {
        int fit = 0;
        for (int i = 0; i < inventorySlots; i++)
        {
            if (items[i] == null)
            {
                if (limits[i] == _item.filterType || limits[i] == "" || limits[i] == null) {
                    fit += _item.stackSize;
                }
            }
            else if (items[i].Name == _item.Name)
            {
                fit += _item.stackSize - amounts[i];
            }
        }
        return fit;
    }
    public int GetEmptySlot(string _filter)
    {
       // returns -1 if there is no empty slots
       // otherwise returns the index of the first empty slot
        int emptySlot = -1;
        for (int i = 0; i < inventorySlots; i++)
        {
            if (items[i] == null && (limits[i] == null || limits[i] == "" || limits[i] == _filter))
            {
                emptySlot = i;
                break;
            }
        }
        return emptySlot;
    }
    public int GetEmptySlotNumber()
    {
        // returns the number of empty slots
        int emptySlots = 0;
        for (int i = 0; i < inventorySlots; i++)
        {
            if (items[i] == null)
            {
                emptySlots++;
            }
        }
        return emptySlots;
    }
    public void ClearInventory()
    {
        for (int i = 0; i < inventorySlots; i++)
        {
            items[i] = null;
            amounts[i] = 0;
        }
        InventoryUpdate();
    }
    public void InventoryUpdate()
    {
        displayUpdate = true;
        GameObject craftingWindow = GameObject.Find("CraftingWindow");
        if (craftingWindow != null)
        {
            craftingWindow.GetComponent<CraftingWindow>().OnPageUpdate();
        }
    }
}