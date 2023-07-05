using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButtonDeselect : MonoBehaviour
{
	[SerializeField] GameObject[] objectsToHide;
	[SerializeField] GameObject[] objectsToShow;

	public void ToggleButtonVisuals()
    {
	    foreach(GameObject obj in objectsToHide)
	    {
	    	obj.SetActive(false);
	    }
	    
	    foreach(GameObject obj in objectsToShow)
	    {
	    	obj.SetActive(true);
	    }
    }
}
