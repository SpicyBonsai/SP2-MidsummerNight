using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardPopup : MonoBehaviour
{
    private Camera _cam;
    void Start()
    {
        _cam = Camera.main;
    }

    
    void Update()
    {
        transform.forward = transform.position - _cam.transform.position;
    }
}
