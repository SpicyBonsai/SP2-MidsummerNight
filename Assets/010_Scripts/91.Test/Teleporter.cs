using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.GetComponent<NavMeshAgent>().enabled = false;
            other.transform.position = destination.position;
            other.GetComponent<NavMeshAgent>().enabled = true;
            other.GetComponent<CharacterController>().enabled = true;
        }
    }
}
