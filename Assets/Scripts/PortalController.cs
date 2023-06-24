using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] List<GameObject> hiddenObjects = new List<GameObject>();
    [SerializeField] List<GameObject> visibleObjects = new List<GameObject>();

    List<GameObject> objectsToHide = new List<GameObject>();
    List<GameObject> objectsToShow = new List<GameObject>();

    private Vector3 playerPosition;
    private Vector3 lastPlayerPosition;

    private void Update() 
    {
        if(objectsToHide.Count > 0)
        {
            foreach(GameObject obj in objectsToHide)
            {
                hiddenObjects.Add(obj);
                visibleObjects.Remove(obj);
            }
        }   
        if(objectsToShow.Count > 0)
        {
            foreach(GameObject obj in objectsToShow)
            {
                visibleObjects.Add(obj);
                hiddenObjects.Remove(obj);
            }
        } 

        objectsToShow.Clear();
        objectsToHide.Clear();
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


            // if (Vector3.Dot(lastPlayerPosition - playerPosition, transform.up) > 0)
            // {
                
            // }

            foreach (GameObject obj in hiddenObjects)
            {
                obj.GetComponent<ChangeObjectOpacity>().DisolveIn();
                objectsToShow.Add(obj);
            }

            foreach (GameObject obj in visibleObjects)
            {
                obj.GetComponent<ChangeObjectOpacity>().DisolveOut();
                objectsToHide.Add(obj);
            }



            // else
            // {
            //     foreach (GameObject obj in hiddenObjects)
            //     {
            //         obj.GetComponent<ChangeObjectOpacity>().DisolveIn();
            //         obj.GetComponent<MeshRenderer>().enabled = false;
            //     }

            //     foreach (GameObject obj in objectsToHide)
            //     {
            //         obj.GetComponent<ChangeObjectOpacity>().DisolveOut();
            //         obj.GetComponent<MeshRenderer>().enabled = true;
            //     }
            // }
        }
        //if player position minus previous player position move in the direction of the portal's normal, then do something
    }
}
