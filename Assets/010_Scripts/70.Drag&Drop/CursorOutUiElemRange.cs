using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorOutUiElemRange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        //InputManager.GetInstance().SwitchToUI();
        _mouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //InputManager.GetInstance().SwitchToGameplay();
        _mouseOverUI = false;
    }


}
