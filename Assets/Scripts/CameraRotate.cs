using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraRotate : MonoBehaviour
{
    public float Sensitivity, MaxVerticalRotation = 60f, MinCamScale, MaxCamScale;
    public CinemachineVirtualCamera Cam;
    float _targetRotation,
          _lenseDistanceMultiplier,
          _btnPressed = 0;

    Ray ray;
    RaycastHit hit;

    void Update()
    {
        #region If right mouse btn down 
        if (Input.GetMouseButtonDown(1))
        {
            _btnPressed = 1;
        }
        else if(Input.GetMouseButtonUp(1))
        {
            _btnPressed = 0;
        }
        #endregion

        #region Camera rotation horizontally
        _targetRotation += Input.GetAxis("Mouse X") * Sensitivity * _btnPressed;
        _targetRotation += Input.GetAxis("RightJoystick_X") * Sensitivity;
        transform.localEulerAngles = new Vector3(0, _targetRotation, 0);
        #endregion

        #region Scale up/down when scrolling a mousewheel
        if (Input.mouseScrollDelta.y > 0)
            _lenseDistanceMultiplier = -100f;
        else if (Input.mouseScrollDelta.y < 0)
            _lenseDistanceMultiplier = 100f;
        else
            _lenseDistanceMultiplier = 0;

        Cam.m_Lens.FieldOfView += _lenseDistanceMultiplier * Sensitivity * Time.deltaTime;
        Cam.m_Lens.FieldOfView = Mathf.Clamp(Cam.m_Lens.FieldOfView, MinCamScale, MaxCamScale);
        //print(Cam.m_Lens.FieldOfView);
        #endregion
    }
}
