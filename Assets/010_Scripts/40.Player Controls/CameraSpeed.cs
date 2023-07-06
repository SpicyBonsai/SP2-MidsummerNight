using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSpeed : MonoBehaviour
{
    private CinemachineVirtualCamera cam1;
    private CinemachineTransposer camTransposer;
    private CinemachineComposer camComposer;
    Transform playerPos;
    Transform cameraPos;
    float dotCameraPlayer;

    [SerializeField] float xDampingWhenHold;
    [SerializeField] float yDampingWhenHold;
    [SerializeField] float yawDampingWhenHold = 0.3f;
    [SerializeField] float horizontalDampingWhenHold;
    [SerializeField] float verticalDampingWhenHold;
    
    void Start()
    {
        cam1 = gameObject.GetComponent<CinemachineVirtualCamera>();
        cameraPos = Camera.main.transform;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        camTransposer = cam1.GetCinemachineComponent<CinemachineTransposer>();
        camComposer = cam1.GetCinemachineComponent<CinemachineComposer>();
    }

    
    void Update()
    {
        //cameraPos.position = new Vector3(cameraPos.position.x, playerPos.position.y, cameraPos.position.z);
        //Debug.Log("Dot product of camera and player forwards:" + Vector3.Dot(cameraPos.forward.normalized, playerPos.forward));
        dotCameraPlayer = Vector3.Dot(cameraPos.forward.normalized, playerPos.forward);

        camTransposer.m_XDamping = Mathf.Clamp(2 * dotCameraPlayer, 0, 2);
        camTransposer.m_YDamping = Mathf.Clamp(2 * dotCameraPlayer, 0, 2);
        camTransposer.m_YawDamping = Mathf.Clamp(7 * dotCameraPlayer, 0, 7);

        camComposer.m_HorizontalDamping = Mathf.Clamp(2 * dotCameraPlayer, 0, 2);
        camComposer.m_VerticalDamping = Mathf.Clamp(2 * dotCameraPlayer, 0, 2);


        //TODO replace the input
        if(InputManager.GetInstance().RightClick)
        {
            camTransposer.m_XDamping = xDampingWhenHold;
            camTransposer.m_YDamping = yDampingWhenHold;
            camTransposer.m_YawDamping = yawDampingWhenHold;

            camComposer.m_HorizontalDamping = horizontalDampingWhenHold;
            camComposer.m_VerticalDamping = verticalDampingWhenHold;
        }
    }
}
