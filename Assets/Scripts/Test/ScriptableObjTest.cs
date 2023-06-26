using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjTest", menuName = "SP2-MidsummerNight/ScriptableObjTest", order = 0)]
public class ScriptableObjTest : ScriptableObject 
{
    public virtual void OnCall()
    {
        Debug.Log("Customize this");
    }    

    Event myEvent;
}