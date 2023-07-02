using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("To use: ")]
    [Header("FindObjectOfType<AudioManager>().Play(track name);")]
    public Sound[] SoundsList;
    public static AudioManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound _s in SoundsList)
        {
            _s.Source = gameObject.AddComponent<AudioSource>();
            _s.Source.clip = _s.Clip;

            _s.Source.volume = _s.Volume;
            _s.Source.pitch = _s.Pitch;
            _s.Source.loop = _s.Loop;
        }
    }

    public void Play (string _name)
    {
         Sound _s = Array.Find(SoundsList, Sound => Sound.Name == _name);
        if (_s == null)
            return;

        _s.Source.Play();
    }

    public void Pause (string _name)
    {
        Sound _s = Array.Find(SoundsList, Sound => Sound.Name == _name);
        if (_s == null)
            return;

        _s.Source.Pause();
    }

    public void Stop(string _name)
    {
        Sound _s = Array.Find(SoundsList, Sound => Sound.Name == _name);
        if (_s == null)
            return;

        _s.Source.Stop();
    }
}
