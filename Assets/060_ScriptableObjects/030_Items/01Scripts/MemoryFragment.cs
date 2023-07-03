using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryFragment : MonoBehaviour
{
    public ItemObject ItemObj;
    public OnTriggerEnterEvent EnterSoundCollider;
    //public string SoundName;
    bool _soundON = false;

    private void Update()
    {
        if (EnterSoundCollider.PlayerEnteredObject && !_soundON)
        {
            GetComponent<AudioSource>().Play();
            _soundON = true;
        }
        else if (!EnterSoundCollider.PlayerEnteredObject)
        {
            _soundON = false;
            GetComponent<AudioSource>().Stop();
        }    
    }

}
