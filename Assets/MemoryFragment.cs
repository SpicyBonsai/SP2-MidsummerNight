using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryFragment : MonoBehaviour
{
    public ItemObject Item;
    public string SoundName;

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<AudioManager>().Play("FragmentNearby");
        print("Sound should playing");
    }

    private void OnTriggerExit(Collider other)
    {
        FindObjectOfType<AudioManager>().Stop("FragmentNearby");
    }
}
