using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemAsset", menuName = "Item Asset")]
public class ItemInfo : ScriptableObject
{
    public Item item;
    public bool stackable;
    public string itemName;
    public string description;
    public Sprite sprite;
}
