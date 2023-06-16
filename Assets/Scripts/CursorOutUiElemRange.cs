using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorOutUiElemRange : MonoBehaviour
{
    public bool MouseOverUI = false;

    private void OnMouseOver()
    {
        MouseOverUI = true;
    }

    private void OnMouseExit()
    {
        MouseOverUI = false;
    }
}
