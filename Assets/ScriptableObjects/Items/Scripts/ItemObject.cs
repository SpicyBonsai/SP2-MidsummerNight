using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    World,
    Memory
}

public abstract class ItemObject : ScriptableObject
{
    public int ID;
    public Sprite ItemSprite;
    public GameObject ItemPrefab;
    [HideInInspector]
    public ItemType ItemObjType;
    [TextArea(15, 20)]
    public string Description;
}

[System.Serializable]
public class Item
{
    public string Name;
    public int ID;
    public Item(ItemObject _item)
    {
        Name = _item.name;
        ID = _item.ID;
    }
}
