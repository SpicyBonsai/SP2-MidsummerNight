using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldItem", menuName = "Inventory System/Items/World")]
public class WorldItem : ItemObject
{
    private void Awake()
    {
        ItemObjType = ItemType.World;
    }
}
