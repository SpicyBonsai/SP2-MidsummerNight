using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoverHiddenObjects : MonoBehaviour
{
    [SerializeField]
    private Transform[] _hiddenObjects;
    private Transform _cameraPosition;

    void Start()
    {
        _cameraPosition = Camera.main.transform;
    }

    void Update()
    {
        foreach(Transform _objPosition in _hiddenObjects)
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(_cameraPosition.position, _objPosition.position + Vector3.up - _cameraPosition.position);
            Debug.DrawRay(_cameraPosition.position, _objPosition.position + Vector3.up - _cameraPosition.position);

            bool passedPortal = false;
            
            
            for(int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                //Debug.Log(hit.transform.tag);

                if(hit.transform.tag == "Portal")
                {
                    passedPortal = true;
                }

                if (passedPortal && hit.transform.tag == "HiddenObject")
                {
                    hit.transform.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }else if(!passedPortal && hit.transform.tag == "HiddenObject")
                {
                    hit.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
            }

        }
    }
}
