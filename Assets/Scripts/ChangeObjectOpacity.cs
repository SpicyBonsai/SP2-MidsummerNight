using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjectOpacity : MonoBehaviour
{
    public float desiredTime = 0.5f;
    float lerpResult, elapsedTime;
    bool isDissolvingIn = false;
    bool isDissolvingOut = false;
    MeshRenderer objMesh;

    private void Awake() 
    {
       objMesh = gameObject.GetComponent<MeshRenderer>();
    }


    void Update()
    {
        elapsedTime += Time.deltaTime;
        lerpResult = elapsedTime/desiredTime;

        if (isDissolvingIn)
        {
            
            if (objMesh.material.HasFloat("_Dissolve"))
            {
                objMesh.enabled = true;
                lerpResult = 1 - Mathf.Clamp(lerpResult, 0f, 1f);
                objMesh.material.SetFloat("_Dissolve", lerpResult);
            }

            if(lerpResult <= 0)
            {
                isDissolvingIn = false;
            }
        }

        if (isDissolvingOut)
        {
            if (objMesh.material.HasFloat("_Dissolve"))
            {
                lerpResult = Mathf.Clamp(lerpResult, 0f, 1f);
                //Debug.Log(lerpResult);
                objMesh.material.SetFloat("_Dissolve", lerpResult);
            }

            if(lerpResult >= 1)
            {
                isDissolvingOut = false;
                objMesh.enabled = false;
            }
            
        }


    }

    public void DisolveIn()
    {
        elapsedTime = 0;
        isDissolvingIn = true;
    }
    public void DisolveOut()
    {
        elapsedTime = 0;
        isDissolvingOut = true;

    }
}
