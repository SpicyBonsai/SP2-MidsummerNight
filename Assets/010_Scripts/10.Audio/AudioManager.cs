using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private GameOptions _gameOptions;
    [SerializeField] public AudioMixer master;

    #region Singleton Mumbo Jumbo
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
        DontDestroyOnLoad(gameObject);

        SetMasterVolume(_gameOptions.OverallSound);
        SetSFXVolume(_gameOptions.SfxVolume);
        SetMusicVolume(_gameOptions.MusicVolume);
        SetAmbientVolume(_gameOptions.AmbientSound);
    }
    #endregion

    public void ValuesChanged()
    {
        SetMasterVolume(_gameOptions.OverallSound);
        SetSFXVolume(_gameOptions.SfxVolume);
        SetMusicVolume(_gameOptions.MusicVolume);
        SetAmbientVolume(_gameOptions.AmbientSound);
    }

    public void SetMasterVolume(float volume)
    {
        master.SetFloat("Master", Mathf.Log10 (volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        master.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        master.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void SetAmbientVolume(float volume)
    {
        master.SetFloat("Ambient", Mathf.Log10(volume) * 20);
    }
}
