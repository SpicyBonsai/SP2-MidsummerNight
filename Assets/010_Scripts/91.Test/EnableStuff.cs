using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableStuff : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToEnable;
    
    public void Enable()
    {
        foreach (GameObject gameObj in objectsToEnable)
        {
            gameObj.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Enable();
        }
    }
}
