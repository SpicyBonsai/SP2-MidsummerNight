using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PinVisibility : MonoBehaviour
{
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float minimumDistance;
    private MeshRenderer _pinRenderer;
    void Start()
    {
        // _playerTransform = GameObject.FindWithTag("Player").transform;
        _pinRenderer = GetComponent<MeshRenderer>();
    }

    
    void Update()
    {
        if (Vector3.Distance(transform.position, groundCheckTransform.position) < minimumDistance)
        {
            _pinRenderer.enabled = false;
        }
        else
        {
            _pinRenderer.enabled = true;
        }
    }
}
