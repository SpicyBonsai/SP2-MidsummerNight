using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private CinemachineVirtualCamera cam1;
    [SerializeField] private CameraControls controls;

    private CinemachineTransposer camTransposer;
    private CinemachineComposer camComposer;

    private void Start()
    {
        // cam1 = Camera.main.GetComponent<CinemachineVirtualCamera>();
        camTransposer = cam1.GetCinemachineComponent<CinemachineTransposer>();
        camComposer = cam1.GetCinemachineComponent<CinemachineComposer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            controls.isteleporting = true;

            other.GetComponent<CharacterController>().enabled = false;
            other.GetComponent<NavMeshAgent>().enabled = false;
            other.transform.position = destination.position;
            other.GetComponent<NavMeshAgent>().enabled = true;
            other.GetComponent<CharacterController>().enabled = true;
        }
    }
}
