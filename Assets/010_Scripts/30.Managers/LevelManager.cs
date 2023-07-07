using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] GameOptions gameOptions;

    private void Awake() 
    {
        #region Singleton Instance Setup
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
        
    }

    private void Start() 
    {
        //Initialize the values just in case they haven't been set yet
        ValuesChanged();
    }

    private void Update() 
    {
        // if (InputManager.GetInstance().Submit)
        // {
        //     Debug.Log("TextSpeed " + PlayerPrefs.GetFloat("TextSpeed"));
        //     Debug.Log("Fullscreen " + PlayerPrefs.GetInt("Fullscreen"));
        //     Debug.Log("Windowed " + PlayerPrefs.GetInt("Windowed", 0));
        //     Debug.Log("SubtitlesOn " + PlayerPrefs.GetInt("SubtitlesOn", 1));
        //     Debug.Log("SubtitlesOff " + PlayerPrefs.GetInt("SubtitlesOff", 0));
            
        //     Debug.Log("Overall " + PlayerPrefs.GetFloat("OverallSound"));
        //     Debug.Log("Ambient " + PlayerPrefs.GetFloat("AmbientSound"));
        //     Debug.Log("Music " + PlayerPrefs.GetFloat("MusicVolume"));
        //     Debug.Log("SFX " + PlayerPrefs.GetFloat("SfxVolume"));
        // }    
    }


    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }


    //this gets called when the player changes either the slider or the toggle values
    //this keeps the values up to date so we can access them from anywhere
    public void ValuesChanged()
    {
        gameOptions.SetTextSpeed(PlayerPrefs.GetFloat("TextSpeed", 0.5f));
        gameOptions.SetFullscreen(PlayerPrefs.GetInt("Fullscreen") == 1 ? true : false);
        gameOptions.SetWindowed(PlayerPrefs.GetInt("Windowed") == 1 ? true : false);
        gameOptions.SetSubtitlesOn(PlayerPrefs.GetInt("SubtitlesOn", 1) == 1 ? true : false);
        gameOptions.SetSubtitlesOff(PlayerPrefs.GetInt("SubtitlesOff", 0) == 1 ? true : false);
        gameOptions.SetOverallSound(PlayerPrefs.GetFloat("OverallSound", 0.5f));
        gameOptions.SetAmbientSound(PlayerPrefs.GetFloat("AmbientSound", 0.5f));
        gameOptions.SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.5f));
        gameOptions.SetSfxVolume(PlayerPrefs.GetFloat("SfxVolume", 0.5f));
    }

    public void ChangeSliderValue(string name, float value)
    {
        PlayerPrefs.SetFloat(name, value);
        ValuesChanged();
    }

    public void ChangeToggleValue(string name, bool value)
    {
        PlayerPrefs.SetInt(name, value ? 1 : 0);
        ValuesChanged();
    }


    //these get called on the start of the game
    //could use them for gameplay as well, but I'm not sure if we want to
    //since it's more prone to errors as you access the values using strings
    public float GetSliderValue(string name)
    {
        return PlayerPrefs.GetFloat(name);
    }

    public bool GetToggleValue(string name, int preference = 0)
    {
        return PlayerPrefs.GetInt(name, preference) == 1 ? true : false;
    }
}
