using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Item
{
    NONE,
    MUSHROOM
}

[System.Serializable]
public struct ItemEventPair
{
    public Item item;
    public string blockName;

    public ItemEventPair(Item _item, string _blockName)
    {
        item = _item;
        blockName = _blockName;
    }
}

public class ItemDatabase
{
    private static ItemInfo[] itemDatabase;

    public static void Init()
    {
        itemDatabase = Resources.LoadAll<ItemInfo>("Items");
    }

    public static ItemInfo GetInfo(Item item)
    {
        foreach (ItemInfo itemInfo in itemDatabase)
        {
            if (itemInfo.item == item)
                return itemInfo;
        }
        Debug.LogError("No item asset can be found for that item enum.");
        return null;

    }

    public static ItemInfo GetInfo(string name)
    {
        foreach (ItemInfo itemInfo in itemDatabase)
        {
            if (itemInfo.itemName == name)
                return itemInfo;
        }
        Debug.LogError("No item asset can be found for that item name.");
        return null;
    }
}