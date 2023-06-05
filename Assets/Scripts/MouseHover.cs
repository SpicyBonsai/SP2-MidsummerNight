using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover : MonoBehaviour
{
    public static GameObject HoveredObj
    {
        get { return _hoveredObj; }
    }

    static GameObject _hoveredObj;

    Ray _ray;
    RaycastHit _hit;

    void Update()
    {

        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hit))
        {
            _hoveredObj = _hit.collider.transform.gameObject;
        }
    }

}
