using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private bool _isToggled = false;
    // [SerializeField] private bool isOneWay = false;
    public GameObject _toggleGraphic;
    
    void Start()
    {
        //set up the graphics
        if(_isToggled)
        {
            _toggleGraphic.SetActive(true);
        }
        else
        {
            _toggleGraphic.SetActive(false);
        }
    }

    
    public void Toggle()
    {
        // if(isOneWay && _isToggled)
        // {
        //     return;
        // }

        if (_isToggled)
        {
            _isToggled = false;
            _toggleGraphic.SetActive(false);
        }
        else
        {
            _isToggled = true;
            _toggleGraphic.SetActive(true);
        }
    }
}
