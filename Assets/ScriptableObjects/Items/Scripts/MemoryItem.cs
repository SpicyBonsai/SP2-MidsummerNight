using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MemoryItem", menuName = "Inventory System/Items/Memory")]
public class MemoryItem : ItemObject
{
    private void Awake()
    {
        ItemObjType = ItemType.Memory;
    }
}
