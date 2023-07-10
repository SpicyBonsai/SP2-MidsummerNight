using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer master;

    public void SetLevel(float sliderValue)
    {
        master.SetFloat("Music", Mathf.Log10 (sliderValue) * 20);
    }
}
