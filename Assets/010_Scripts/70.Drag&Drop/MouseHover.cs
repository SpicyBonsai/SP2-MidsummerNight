using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHover : MonoBehaviour
{
    public static GameObject HoveredObj
    {
        get { return _hoveredObj; }
    }
    static GameObject _hoveredObj;

    //public static bool CursorIsOverUI;
    public static bool CursorIsOverUI
    {
        get { return _cursorIsOverUI; }
    }
    static bool _cursorIsOverUI;

    Ray _ray;
    RaycastHit _hit;

    void Update()
    {
        _ray = Camera.main.ScreenPointToRay(InputManager.GetInstance().MousePosition);
        if (Physics.Raycast(_ray, out _hit))
        {
            _hoveredObj = _hit.collider.transform.gameObject;
        }
        _cursorIsOverUI = IsPointerOverUIObject();
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(InputManager.GetInstance().MousePosition.x, InputManager.GetInstance().MousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        foreach (RaycastResult result in results)
            Debug.Log(result);
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

}
