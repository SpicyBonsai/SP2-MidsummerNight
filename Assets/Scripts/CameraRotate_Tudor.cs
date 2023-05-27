using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate_Tudor : MonoBehaviour
{
    public float Sensitivity;

    float _targetRotation;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            _targetRotation += Input.GetAxis("Mouse X") * Sensitivity;
            _targetRotation += Input.GetAxis("RightJoystick_X") * Sensitivity;

            transform.localEulerAngles = new Vector3(0, _targetRotation, 0);
        }
        else{
            Cursor.lockState = CursorLockMode.None; 
        }

    }
}
