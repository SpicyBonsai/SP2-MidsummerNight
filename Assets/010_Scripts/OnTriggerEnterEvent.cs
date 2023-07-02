using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterEvent : MonoBehaviour
{
    public bool PlayerEnteredObject
    {
        get { return _playerEntered; }
    }
    bool _playerEntered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        else
            _playerEntered = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerEntered = false;

    }
}
