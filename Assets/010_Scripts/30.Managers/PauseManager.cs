using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    public bool IsPaused { get; private set; }

    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }   

        EnableInventory();
        EnableMemoryMenu();
    }

    public bool canOpenInventory { get; set; }
    public bool canOpenMemoryMenu { get; set; }
    public bool InventoryOpen = false;

    [SerializeField] private GameObject _inventoryMenu = null;
    [SerializeField] private GameObject _memoryMenu = null;

    private Animator _animatorInventory;
    private Animator _animatorMemory;

    private void Start() 
    {
        _animatorInventory = _inventoryMenu.GetComponent<Animator>();
        _animatorMemory = _memoryMenu.GetComponent<Animator>();    
    }

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;

        InputManager.PlayerInput.SwitchCurrentActionMap("UI");
        Debug.Log(InputManager.PlayerInput.currentActionMap);
        //PlayerInput.instance.SwitchCurrentActionMap("Dialogue");
        // Debug.Log(InputManager.GetInstance().PlayerInput.currentActionMap);
    }

    public void UnpauseGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        
        InputManager.PlayerInput.SwitchCurrentActionMap("Gameplay");
        Debug.Log(InputManager.PlayerInput.currentActionMap);
        // InputManager.GetInstance().PlayerInput.SwitchCurrentActionMap("Gameplay");
        // Debug.Log(InputManager.GetInstance().PlayerInput.currentActionMap);
    }

    public void OpenInventory()
    {
        if(InventoryOpen)
        {
            return;
        }

        if(canOpenInventory || canOpenMemoryMenu)
        {
            InputManager.GetInstance().SwitchToUI();
        }

        if(canOpenInventory)
        {
            _inventoryMenu.SetActive(true);
            _animatorInventory.SetTrigger("Open");
        }

        if(canOpenMemoryMenu)
        {
            _memoryMenu.SetActive(true);
            _animatorMemory.SetTrigger("Open");
        }

        InventoryOpen = true;
    }

    public void CloseInventory()
    {
        if(!InventoryOpen)
        {
            return;
        }

        if(canOpenInventory || canOpenMemoryMenu)
        {
            InputManager.GetInstance().SwitchToGameplay();
        }

        if(canOpenInventory)
        {
            _animatorInventory.SetTrigger("Close");
            if(_animatorInventory.GetCurrentAnimatorStateInfo(0).IsName("CloseInventory") && _animatorInventory.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                _inventoryMenu.SetActive(false);
            }
        }

        if(canOpenMemoryMenu)
        {
            _animatorMemory.SetTrigger("Close");
            if(_animatorInventory.GetCurrentAnimatorStateInfo(0).IsName("CloseMemory") && _animatorMemory.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                _memoryMenu.SetActive(false);
            }
        }

        InventoryOpen = false;
    }

    public void EnableInventory()
    {
        canOpenInventory = true;
    }

    public void EnableMemoryMenu()
    {
        canOpenMemoryMenu = true;
    }

}
