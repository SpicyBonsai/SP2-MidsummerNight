using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float Sensitivity;

    float _targetRotation;

    void Update()
    {
        _targetRotation += Input.GetAxis("Mouse X") * Sensitivity;
        _targetRotation += Input.GetAxis("RightJoystick_X") * Sensitivity;

        transform.localEulerAngles = new Vector3(0, _targetRotation, 0);
    }
}
