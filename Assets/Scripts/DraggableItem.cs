using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventoryObject Inventory;
    [HideInInspector]
    public Transform ParentAfterDrag;
    public GameObject ObjToInstantiate;
    public Camera Cam;

    [Tooltip("Name of part on world Object")]
    public string ItemName;
    //public float _distanceFromCam;

    bool _hoveringUI = false,
         _3DcursorInstance = false; //to instantiate 3D object at cursor pos. only once

    GameObject _instantiatedObj;

    //public Camera Cam; //uncomment for 3D screen

    public void OnBeginDrag(PointerEventData eventData)
    {
        //print("begin dragging");
        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        gameObject.GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //print(" dragging");
        #region Uncomment for 3D canvas space screen
/*                Vector3 _screenPoint = Input.mousePosition;
                _screenPoint.z = 60f;//distance of the plane from the camera
                transform.position = Cam.ScreenToWorldPoint(_screenPoint);*/
        #endregion
        transform.position = Input.mousePosition;
        _hoveringUI = eventData.pointerCurrentRaycast.gameObject ? true : false;

        if (!_hoveringUI)
        {
            Vector3 _screenPoint = Input.mousePosition;
            _screenPoint.z = 5.84f;
            Vector3 _instantiatePos = Camera.main.ScreenToWorldPoint(_screenPoint);
            
            if (!_3DcursorInstance)
            {
                _instantiatedObj = Instantiate(ObjToInstantiate, _instantiatePos, ObjToInstantiate.transform.rotation);
                _3DcursorInstance = true;
            }
            _instantiatedObj.transform.position = _instantiatePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        /*        Ray _ray;
                RaycastHit _hit;
                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out _hit))
                {
                    print(_hit.collider.name) ;
                }*/
            
        //transform.SetParent(ParentAfterDrag);
        _hoveringUI = eventData.pointerCurrentRaycast.gameObject ? true : false;
        //print(_hoveringUI);
        if (_hoveringUI)
            return;
        else if(!_hoveringUI) //&& MouseHover.HoveredObj.name == ItemName
        {
            //Vector3 _instantiatePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //_instantiatePos.z = _distanceFromCam;
            #region Instantiate dropped object
            /* Vector3 _screenPoint = Input.mousePosition;

             _screenPoint.z = 5.84f;
             Vector3 _instantiatePos = Camera.main.ScreenToWorldPoint(_screenPoint);
             Instantiate(ObjToInstantiate, _instantiatePos, ObjToInstantiate.transform.rotation);*/
            #endregion

            MouseHover.HoveredObj.GetComponent<MeshRenderer>().enabled = true;
        }
        transform.SetParent(ParentAfterDrag);
        gameObject.GetComponent<Image>().raycastTarget = true;
    }



}
