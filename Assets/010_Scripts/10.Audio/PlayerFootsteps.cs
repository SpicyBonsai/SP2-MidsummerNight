using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = AudioManager.Instance;
    }

    public void PlayFootstepSound()
    {
        _audioManager.RandomSoundEffect(_audioManager.footsteps);
    }
}
