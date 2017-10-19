using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Item
{
    NONE
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

[CreateAssetMenu(fileName ="ItemAsset", menuName ="Item Asset")]
public class ItemInfo : ScriptableObject
{
    public Item item;
    public string itemName;
    public string description;
    public Sprite sprite;
}

public class ItemDatabase
{
    private static ItemInfo[] itemDatabase;

    public static void Init()
    {
        itemDatabase = Resources.LoadAll<ItemInfo>("Resources/Items");
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
}