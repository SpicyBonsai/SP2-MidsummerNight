/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount > 0)
            return;
        else
        {
            GameObject dropped = eventData.pointerDrag;
            dropped.GetComponent<DraggableItem>().ParentAfterDrag = transform;
        }
    }
}
*/