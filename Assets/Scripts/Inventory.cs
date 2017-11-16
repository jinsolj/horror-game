using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory {
    List<InventoryItem> inventory;

    public delegate void OnAddItem(Item item);
    public delegate void OnRemoveItem(Item item);

    public event OnAddItem AddItemEvent;
    public event OnRemoveItem RemoveItemEvent;
        
    public Inventory()
    {
        inventory = new List<InventoryItem>();
    }

    public InventoryItem FindItem(Item item)
    {
        foreach(InventoryItem inventoryItem in inventory)
        {
            if(inventoryItem.item == item)
            {
                return inventoryItem;
            }
        }
        return null;
    }

    public bool ContainsItem(Item item, int quantity = 1)
    {
        ItemInfo info = ItemDatabase.GetInfo(item);
        InventoryItem inventoryItem = FindItem(item);
        return inventoryItem != null && 
            (inventoryItem.quantity > quantity || !info.stackable);
    }

    public void AddItem(Item item, int quantity = 1)
    {
        if(quantity < 1)
        {
            Debug.LogError("You cannot add a non-positive quantity of an item.");
            return;
        }


        ItemInfo info = ItemDatabase.GetInfo(item);
        if (info.stackable)
        {
            foreach (InventoryItem inventoryItem in inventory)
            {
                if (inventoryItem.item == item)
                {
                    inventoryItem.quantity += quantity;
                    if (AddItemEvent != null)
                        AddItemEvent(item);
                    return;
                }
            }
        }

        inventory.Add(new InventoryItem(item, quantity));
    }

    public void RemoveItem(Item item, int quantity = 1)
    {
        if(quantity < 1)
        {
            Debug.LogError("Cannot remove a non-positive quantity of an item.");
            return;
        }

        ItemInfo info = ItemDatabase.GetInfo(item);
        int totalRemoved = 0;
        foreach (InventoryItem inventoryItem in inventory)
        {
            if (inventoryItem.item == item)
            {
                if (info.stackable)
                {
                    inventory.Remove(inventoryItem);
                    totalRemoved++;
                    if (totalRemoved == quantity)
                    {
                        if (RemoveItemEvent != null)
                            RemoveItemEvent(item);
                        return;
                    }
                }
                else
                {
                    inventoryItem.quantity -= quantity;
                    totalRemoved += quantity;
                    if (inventoryItem.quantity == 0)
                    {
                        inventory.Remove(inventoryItem);
                        if (RemoveItemEvent != null)
                            RemoveItemEvent(item);
                        return;
                    }
                    else if(inventoryItem.quantity < 0)
                    {
                        if (RemoveItemEvent != null)
                            RemoveItemEvent(item);
                        Debug.LogError("Trying to remove more of an item than exists in inventory.");
                        return;
                    }
                }
            }
        }

        Debug.LogError("Either the item does not exist in inventory or you don't have enough of the item you are trying to remove.");
    }

    public class InventoryItem
    {
        public InventoryItem(Item _item, int _quantity)
        {
            item = _item;
            quantity = _quantity;
        }

        public Item item;
        public int quantity;
    }
}

