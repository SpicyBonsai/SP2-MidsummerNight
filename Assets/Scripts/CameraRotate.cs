using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float Sensitivity;

    float _targetRotation, 
          _btnPressed = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _btnPressed = 1;
        }
        else if(Input.GetMouseButtonUp(1))
        {
            _btnPressed = 0;
        }

        _targetRotation += Input.GetAxis("Mouse X") * Sensitivity * _btnPressed;
        _targetRotation += Input.GetAxis("RightJoystick_X") * Sensitivity;

        transform.localEulerAngles = new Vector3(0, _targetRotation, 0);
    }
}
