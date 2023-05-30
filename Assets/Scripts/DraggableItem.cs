using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public Transform ParentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        print("begin dragging");
        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        gameObject.GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        print(" dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("stop dragging");
        transform.SetParent(ParentAfterDrag);
        gameObject.GetComponent<Image>().raycastTarget = true;
    }
}
