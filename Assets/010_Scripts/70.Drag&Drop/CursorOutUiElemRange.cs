using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorOutUiElemRange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event System.Action<bool> MouseOverUIChanged;
    public static bool MouseOverUI { get; private set; }
    private bool _mouseOverUI;

    private void Update()
    {
        MouseOverUI = _mouseOverUI;
        MouseOverUIChanged?.Invoke(MouseOverUI);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _mouseOverUI = true;
        Debug.Log("Hovered panel");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _mouseOverUI = false;
        Debug.Log("Unhovered");
    }
}
