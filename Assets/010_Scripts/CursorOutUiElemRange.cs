using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorOutUiElemRange : MonoBehaviour 
{
    public bool TestBool;
    public static bool MouseOverUI
    {
        get { return _mouseOverUI; }
    }
    static bool _mouseOverUI;
    private void Update()
    {
        //print(MouseOverUI);
        TestBool = _mouseOverUI;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _mouseOverUI = true;
        print("hovered panel");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _mouseOverUI = false;
        print("unhovered");
    }
}
