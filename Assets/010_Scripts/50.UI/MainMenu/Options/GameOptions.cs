using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameOptions", menuName = "ScriptableObjects/GameOptions", order = 1)]
public class GameOptions : ScriptableObject
{
    public float TextSpeed { get; private set;}
    public bool Fullscreen { get; private set; }
    public bool Windowed { get; private set; }
    public bool SubtitlesOn { get; private set; }
    public bool SubtitlesOff { get; private set; }

    public float OverallSound { get; private set; }
    public float AmbientSound { get; private set; }
    public float MusicVolume { get; private set; }
    public float SfxVolume { get; private set; }

    public void SetTextSpeed(float value)
    {
        TextSpeed = value;
    }

    public void SetFullscreen(bool value)
    {
        Fullscreen = value;
    }

    public void SetWindowed(bool value)
    {
        Windowed = value;
    }

    public void SetSubtitlesOn(bool value)
    {
        SubtitlesOn = value;
    }

    public void SetSubtitlesOff(bool value)
    {
        SubtitlesOff = value;
    }

    public void SetOverallSound(float value)
    {
        OverallSound = value;
    }

    public void SetAmbientSound(float value)
    {
        AmbientSound = value;
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = value;
    }

    public void SetSfxVolume(float value)
    {
        SfxVolume = value;
    }
}
