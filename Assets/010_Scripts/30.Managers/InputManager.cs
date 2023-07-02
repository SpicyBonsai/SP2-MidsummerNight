using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    //Public Properties for Input Actions to be accessed by other scripts (e.g. PlayerController)
    public bool InteractButtonPressed { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public bool MenuOpenInput { get; private set; }
    public bool UIMenuCloseInput { get; private set; }
    public bool Submit { get; private set; }
    public bool LeftClick { get; private set; }
    public bool ResetDialogue { get; private set; }
    public bool SaveButtonPressed { get; private set; }

    //Object instance of the PlayerInput component attached to this GameObject (this is a singleton)
    public static PlayerInput PlayerInput;
    
    private InputAction _interactPressed;
    private InputAction _mousePosition;
    private InputAction _menuOpenAction;
    private InputAction _UIMenuCloseAction;
    private InputAction _submit;
    private InputAction _leftClick;
    private InputAction _resetDialogue;
    private InputAction _saveButtonPressed;




    #region Singleton Pattern Setup
    private static InputManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Input Manager in the scene.");
        }
        instance = this;
        
        PlayerInput = gameObject.GetComponent<PlayerInput>();    
        PlayerInput.SwitchCurrentActionMap("Gameplay");

        _interactPressed = PlayerInput.actions["Click"];
        _mousePosition = PlayerInput.actions["MousePosition"];
        _menuOpenAction = PlayerInput.actions["MenuOPEN"];
        _UIMenuCloseAction = PlayerInput.actions["MenuCLOSE"];
        _submit = PlayerInput.actions["Submit"];
        _leftClick = PlayerInput.actions["Click"];
        _resetDialogue = PlayerInput.actions["ResetDialogue"];
        _saveButtonPressed = PlayerInput.actions["SaveButtonPressed"];
    }

    public static InputManager GetInstance() 
    {
        return instance;
    }
    #endregion


    private void Update() 
    {
        
        InteractButtonPressed = _interactPressed.IsPressed();
        MousePosition = _mousePosition.ReadValue<Vector2>();
        MenuOpenInput = _menuOpenAction.WasPerformedThisFrame();
        UIMenuCloseInput = _UIMenuCloseAction.WasPerformedThisFrame();
        Submit = _submit.WasPerformedThisFrame();
        LeftClick = _leftClick.WasPerformedThisFrame();
        ResetDialogue = _resetDialogue.WasPerformedThisFrame();
        SaveButtonPressed = _saveButtonPressed.WasPerformedThisFrame();

    }

    public void SwitchToGameplay()
    {
        PlayerInput.SwitchCurrentActionMap("Gameplay");

    }

    public void SwitchToUI()
    {
        PlayerInput.SwitchCurrentActionMap("UI");

    }





}
