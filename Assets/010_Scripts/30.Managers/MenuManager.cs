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

        if(InputManager.GetInstance().InventoryButton)
        {
            if (!PauseManager.instance.InventoryOpen && InputManager.GetInstance().GetCurrentActionMap() == "Gameplay")
            {
                PauseManager.instance.OpenInventory();
            }
            else
            {
                PauseManager.instance.CloseInventory();
            }
        }

        if (InputManager.GetInstance().UIMenuCloseInput && PauseManager.instance.InventoryOpen)
        {
            PauseManager.instance.CloseInventory();
        }
    }

    public void Pause()
    {
        PauseManager.instance.PauseGame();
        _pauseScreen.SetActive(true);
    }

    public void Unpause()
    {
        PauseManager.instance.UnpauseGame();
        _pauseScreen.SetActive(false);
    }

}
