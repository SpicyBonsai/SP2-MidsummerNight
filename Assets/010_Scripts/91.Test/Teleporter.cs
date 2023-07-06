using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private CinemachineVirtualCamera cam1;
    private CinemachineTransposer camTransposer;
    private CinemachineComposer camComposer;

    private float xDampingWhenHold;
    private float yDampingWhenHold;
    private float yawDampingWhenHold;
    private float horizontalDampingWhenHold;
    private float verticalDampingWhenHold;


    private void Start() {
        // cam1 = Camera.main.GetComponent<CinemachineVirtualCamera>();
        camTransposer = cam1.GetCinemachineComponent<CinemachineTransposer>();
        camComposer = cam1.GetCinemachineComponent<CinemachineComposer>();

        xDampingWhenHold = camTransposer.m_XDamping;
        yDampingWhenHold = camTransposer.m_YDamping;
        yawDampingWhenHold = camTransposer.m_YawDamping;
        horizontalDampingWhenHold = camComposer.m_HorizontalDamping;
        verticalDampingWhenHold = camComposer.m_VerticalDamping;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            camTransposer.m_XDamping = 0;
            camTransposer.m_YDamping = 0;
            camTransposer.m_YawDamping = 0;
            camComposer.m_HorizontalDamping = 0;
            camComposer.m_VerticalDamping = 0;



            other.GetComponent<CharacterController>().enabled = false;
            other.GetComponent<NavMeshAgent>().enabled = false;
            other.transform.position = destination.position;
            other.GetComponent<NavMeshAgent>().enabled = true;
            other.GetComponent<CharacterController>().enabled = true;


            camTransposer.m_XDamping = xDampingWhenHold;
            camTransposer.m_YDamping = yDampingWhenHold;
            camTransposer.m_YawDamping = yawDampingWhenHold;
            camComposer.m_HorizontalDamping = horizontalDampingWhenHold;
            camComposer.m_VerticalDamping = verticalDampingWhenHold;
        }
    }


}
