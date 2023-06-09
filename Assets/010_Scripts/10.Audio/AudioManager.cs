﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[System.Serializable]
public class Audio
{
    [Range(0f, 1f)] public float volume = 1f;
    public AudioClip audioClip;
    public bool isLooping = false;
}

public class AudioManager : MonoBehaviour
{
    //Handles Volume Management of all Audio Sources
    [Header("Volume Management")]
    [SerializeField] private GameOptions _gameOptions;
    [SerializeField] private AudioMixer _master;

    //Audio Sources
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _sfx;
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _ambient;

    //Audio Clips
    [Header("Audio Clips")]
    public Audio[] footsteps;
    public Audio[] uiError;
    public Audio[] uiSuccess;
    public Audio[] uiMainMenuConfirm;
    public Audio[] uiMainMenuExit;
    public Audio[] uiMainMenuHover;
    public Audio[] uiSubMenuHover;
    public Audio[] uiSubMenuConfirm;
    public Audio[] uiPause;
    public Audio[] collectItem;
    public Audio[] collectMemory;
    public Audio[] restoreGate;
    private Dictionary<string, Audio[]> audioClipDict;

    [Header("Songs")]
    public Audio[] gameSongs;
    public Audio[] menuSongs;

    [Header("Ambient")]
    public Audio[] ambientWind;
    public Audio[] ambientDrone;

    #region Singleton Setup
    public static AudioManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioClipDict = new Dictionary<string, Audio[]>
        {
            { "uiError", uiError },
            { "uiSuccess", uiSuccess },
            { "uiMainMenuConfirm", uiMainMenuConfirm },
            { "uiMainMenuExit", uiMainMenuExit },
            { "uiMainMenuHover", uiMainMenuHover },
            { "uiSubMenuHover", uiSubMenuHover },
            { "uiSubMenuConfirm", uiSubMenuConfirm },
            { "uiPause", uiPause }
        };

        //_music = gameObject.GetComponent<AudioSource>();
        //_ambient = gameObject.GetComponent<AudioSource>();
    }

    #endregion
    private void Start()
    {
        ValuesChanged();
        //Janky Temp Implementation
        PlayAmbient(ambientWind);
        PlaySong(gameSongs);
    }

    #region Volume Control
    public void ValuesChanged()
    {
        SetMasterVolume(_gameOptions.OverallSound);
        SetSFXVolume(_gameOptions.SfxVolume);
        SetMusicVolume(_gameOptions.MusicVolume);
        SetAmbientVolume(_gameOptions.AmbientSound);
    }

    public void SetMasterVolume(float volume)
    {
        _master.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        _master.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        _master.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void SetAmbientVolume(float volume)
    {
        _master.SetFloat("Ambient", Mathf.Log10(volume) * 20);
    }
    #endregion

    #region Shuffle Audio Clips List
    private void ShuffleList(List<Audio> list)
    {
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Audio value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    #endregion

    #region Audio Clip Playback
    public void RandomSoundEffect(params Audio[] clips)
    {
        if (clips.Length > 0)
        {
            List<Audio> shuffledList = new List<Audio>(clips);
            ShuffleList(shuffledList);

            _sfx.volume = shuffledList[0].volume;
            _sfx.clip = shuffledList[0].audioClip;
            _sfx.PlayOneShot(_sfx.clip);

            shuffledList.RemoveAt(0);

        }
        else
        {
            Debug.Log("Missing Sound");
        }
    }
    #endregion

    #region Play and Stop Music
    public void PlaySong(Audio[] songs)
    {
        if (songs.Length > 0)
        {
            List<Audio> shuffledList = new List<Audio>(songs);
            ShuffleList(shuffledList);

            _music.volume = shuffledList[0].volume;
            _music.clip = shuffledList[0].audioClip;
            _music.Play();
        }
        else
        {
            Debug.Log("No songs available.");
        }
    }

    public void StopMusic()
    {
        _music.Stop();
    }
    #endregion

    #region Play and Stop Ambient Sound
    public void PlayAmbient(Audio[] ambientSounds)
    {
        if (ambientSounds.Length > 0)
        {
            List<Audio> shuffledList = new List<Audio>(ambientSounds);
            ShuffleList(shuffledList);

            _ambient.volume = shuffledList[0].volume;
            _ambient.clip = shuffledList[0].audioClip;
            _ambient.loop = ambientSounds[0].isLooping;
            _ambient.Play();
        }
        else
        {
            Debug.Log("No ambient sounds available.");
        }
    }

    public void StopAmbient()
    {
        _ambient.Stop();
    }
    #endregion

    #region Call Audio From Events
    public void CallAudio(string audioName)
    {
        if (audioClipDict.ContainsKey(audioName))
        {
            RandomSoundEffect(audioClipDict[audioName]);
        }
        else
        {
            Debug.LogError("Invalid audio name: " + audioName);
        }
    }
    #endregion
}