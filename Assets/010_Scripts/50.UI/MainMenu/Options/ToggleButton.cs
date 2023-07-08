using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private bool _isToggled = false;
    [SerializeField] private string _toggleName;
    public GameObject _toggleGraphic;
    
    void Start()
    {
        _isToggled = LevelManager.instance.GetToggleValue(_toggleName, _isToggled? 1 : 0);

        //set up the graphics
        if(_isToggled)
        {
            _toggleGraphic.SetActive(true);
        }
        else
        {
            _toggleGraphic.SetActive(false);
        }

        // PlayerPrefs.SetInt("SubtitlesOn", 1);
        // PlayerPrefs.SetInt("SubtitlesOff", 0);

    }

    
    public void Toggle()
    {
        _isToggled = !_isToggled;
        _toggleGraphic.SetActive(_isToggled);
        LevelManager.instance.ChangeToggleValue(_toggleName, _isToggled);
        
    }
}
