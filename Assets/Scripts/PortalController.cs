using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] List<GameObject> hiddenObjects = new List<GameObject>();
    [SerializeField] List<GameObject> objectsToHide = new List<GameObject>();
    private Vector3 playerPosition;
    private Vector3 lastPlayerPosition;
    float lerpResult, elapsedTime;
    public float desiredTime = 0.5f;
    MeshRenderer objMesh;
    bool isDissolvingIn = false;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(transform.up);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        lerpResult = elapsedTime/desiredTime;

        if (isDissolvingIn)
        {
            
            if (objMesh.material.HasFloat("_Dissolve"))
            {
                lerpResult = Mathf.Clamp(lerpResult, 0f, 1f);
                objMesh.material.SetFloat("_Dissolve", lerpResult);
            }

            if(lerpResult >= 1)
            {
                objMesh = null;
                isDissolvingIn = false;
            }
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if(other.tag == "Player")
        {
            playerPosition = other.transform.position;
            //Debug.Log("player position = " + other.transform.position);
        }
        //constantly get player's position
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.tag == "Player")
        {
            lastPlayerPosition = other.transform.position;
            //Debug.Log("Last player position = " + other.transform.position);
            if (Vector3.Dot(lastPlayerPosition - playerPosition, transform.up) > 0)
            {
                foreach (GameObject obj in hiddenObjects)
                {
                    elapsedTime = 0;
                    isDissolvingIn = true;
                    objMesh = obj.GetComponent<MeshRenderer>();
                    objMesh.enabled = true;
                }

                foreach (GameObject obj in objectsToHide)
                {
                    obj.GetComponent<MeshRenderer>().enabled = false;
                }
            }
            else
            {
                foreach (GameObject obj in hiddenObjects)
                {
                    obj.GetComponent<MeshRenderer>().enabled = false;
                }

                foreach (GameObject obj in objectsToHide)
                {
                    obj.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
        //if player position minus previous player position move in the direction of the portal's normal, then do something
    }
}
