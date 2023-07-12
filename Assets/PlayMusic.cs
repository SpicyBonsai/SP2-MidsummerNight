using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayAmbient(AudioManager.Instance.ambientWind);
        AudioManager.Instance.PlaySong(AudioManager.Instance.gameSongs);
    }
}
