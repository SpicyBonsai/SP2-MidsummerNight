using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHover : MonoBehaviour
{
    public static GameObject HoveredObj { get; private set; }
    static GameObject _hoveredObj;
    public static bool CursorIsOverUI { get; }
    static bool cursorIsOverUI;
    private Ray _ray;
    private RaycastHit _hit;
    private RaycastHit[] _hits;
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
             //_hoveredObj = _hit.collider.transform.gameObject;
             HoveredObj = _hit.collider.transform.gameObject;
             //_hoverableObject = HoveredObj.GetComponent<IHoverable>();
             //if (_hoverableObject != null)
             //{
             //    _hoverableObject.Interact();
             //}
             
         }
        



        _hits = Physics.RaycastAll(_ray);

        //HoveredObj = _hits[0].collider.transform.gameObject;
        //Debug.Log(HoveredObj);
        foreach (RaycastHit _hit in _hits)
        {
            var _hoverableObj = _hit.collider.transform.gameObject.GetComponent<IHoverable>();
            if (_hoverableObj != null)
            {
                _hoverableObj.Interact();
            }

            if (_hit.collider.transform.gameObject.name == "MemoryFragment")
            {
                HoveredObj = _hit.collider.transform.gameObject;
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
