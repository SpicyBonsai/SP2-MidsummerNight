using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseScreen;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if(InputManager.GetInstance().MenuOpenInput)
        {
            if(!PauseManager.instance.IsPaused)
            {
                Pause();
            }
        }

        else if(InputManager.GetInstance().UIMenuCloseInput)
        {
            if(PauseManager.instance.IsPaused)
            {
                Unpause();
            }
        }
    }

    private void Pause()
    {
        PauseManager.instance.PauseGame();
        _pauseScreen.SetActive(true);
    }

    private void Unpause()
    {
        PauseManager.instance.UnpauseGame();
        _pauseScreen.SetActive(false);
    }

}
