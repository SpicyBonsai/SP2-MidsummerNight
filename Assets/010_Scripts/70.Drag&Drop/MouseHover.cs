using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHover : MonoBehaviour
{
    public static GameObject HoveredObj { get; }
    static GameObject _hoveredObj;
    public static bool CursorIsOverUI { get; }
    static bool cursorIsOverUI;
    private Ray _ray;
    private RaycastHit _hit;
    private Camera _camera;
    private IHoverable _hoverableObject;


    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        //assign ray to go in the direction from screen towards where player is pointing
        _ray = Camera.main.ScreenPointToRay(InputManager.GetInstance().MousePosition);

        //if the raycast hit something, assign the object that it hit to a variable
        if (Physics.Raycast(_ray, out _hit))
        {
            _hoveredObj = _hit.collider.transform.gameObject;
            _hoverableObject = _hoveredObj.GetComponent<IHoverable>();
            if (_hoverableObject != null)
            {
                _hoverableObject?.Interact();
            }
            
        }
        
        
        cursorIsOverUI = IsPointerOverUIObject();
    }

    public static bool IsPointerOverUIObject()
    {
        
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(InputManager.GetInstance().MousePosition.x, InputManager.GetInstance().MousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        // foreach (RaycastResult result in results)
        // {
        //     
        // }
        
        return results.Count > 0;
    }

}
