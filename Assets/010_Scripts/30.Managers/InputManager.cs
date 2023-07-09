using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    [SerializeField] private bool isMenu;

    //Public Properties for Input Actions to be accessed by other scripts (e.g. PlayerController)
    public bool InteractButtonPressed { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public bool MenuOpenInput { get; private set; }
    public bool UIMenuCloseInput { get; private set; }
    public bool Submit { get; private set; }
    public bool LeftClick { get; private set; }
    public bool ResetDialogue { get; private set; }
    public bool SaveButtonPressed { get; private set; }
    public bool RightClick { get; private set; }
    public Vector2 MouseMovement { get; private set; }
    public bool OpenConsole { get; private set; }
    public bool SkipCutscene { get; private set; }
    public bool CancelSkipCutscene { get; private set; }
    public bool InventoryButton { get; private set; }
    public float MouseScroll { get; private set; }


    //Object instance of the PlayerInput component attached to this GameObject (this is a singleton)
    public static PlayerInput PlayerInput;
    
    private InputAction _interactPressed;
    private InputAction _mousePosition;
    private InputAction _menuOpenAction;
    private InputAction _uiMenuCloseAction;
    private InputAction _submit;
    private InputAction _leftClick;
    private InputAction _resetDialogue;
    private InputAction _saveButtonPressed;
    private InputAction _rightClick;
    private InputAction _mouseMovement;
    private InputAction _openConsole;
    private InputAction _skipCutscene;
    private InputAction _inventoryButton;
    private InputAction _mouseScroll;



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

        if(isMenu)
        {
            SwitchToUI();   
        }
        else
        {
            SwitchToGameplay();
        }

        //Assign the InputAction objects to the corresponding InputActionAssets in both action maps
        _openConsole = PlayerInput.actions["OpenConsole"];
        _leftClick = PlayerInput.actions["Click"];
        _saveButtonPressed = PlayerInput.actions["SaveBTN"];
        _rightClick = PlayerInput.actions["RightClick"];
        _skipCutscene = PlayerInput.actions["SkipCutscene"];
        _mousePosition = PlayerInput.actions["MousePosition"];

        //Assign the InputAction objects to the corresponding InputActionAssets in the Gameplay action map
        _menuOpenAction = PlayerInput.actions["MenuOPEN"];
        _mouseMovement = PlayerInput.actions["MouseMovement"];
        _interactPressed = PlayerInput.actions["Click"];
        _mouseScroll = PlayerInput.actions["MouseScroll"];

        //Assign the InputAction objects to the corresponding InputActionAssets in the UI action map
        _submit = PlayerInput.actions["Submit"];
        _resetDialogue = PlayerInput.actions["ResetDialogue"];
        _uiMenuCloseAction = PlayerInput.actions["MenuCLOSE"];
        _inventoryButton = PlayerInput.actions["InventoryButton"];


    }

    public static InputManager GetInstance() 
    {
        return instance;
    }
    #endregion


    private void Update() 
    {

        MousePosition = _mousePosition.ReadValue<Vector2>();
        MenuOpenInput = _menuOpenAction.WasPerformedThisFrame();
        MouseMovement = _mouseMovement.ReadValue<Vector2>();
        InteractButtonPressed = _interactPressed.IsPressed();
        UIMenuCloseInput = _uiMenuCloseAction.WasPerformedThisFrame();
        Submit = _submit.WasPerformedThisFrame();
        ResetDialogue = _resetDialogue.WasPerformedThisFrame();
        LeftClick = _leftClick.WasPerformedThisFrame();
        SaveButtonPressed = _saveButtonPressed.WasPerformedThisFrame();
        RightClick = _rightClick.IsPressed();
        OpenConsole = _openConsole.WasPerformedThisFrame();
        MouseScroll = _mouseScroll.ReadValue<Vector2>().y / 120f;

        SkipCutscene = _skipCutscene.IsPressed();
        CancelSkipCutscene = _skipCutscene.WasReleasedThisFrame();
        InventoryButton = _inventoryButton.WasPerformedThisFrame();


    }

    public void SwitchToGameplay()
    {
        PlayerInput.SwitchCurrentActionMap("Gameplay");
        SwitchCommonMapped();
    }

    public void SwitchToUI()
    {
        PlayerInput.SwitchCurrentActionMap("UI");
        SwitchCommonMapped();
        
    }

    private void SwitchCommonMapped()
    {
        _openConsole = PlayerInput.actions["OpenConsole"];
        _leftClick = PlayerInput.actions["Click"];
        _saveButtonPressed = PlayerInput.actions["SaveBTN"];
        _rightClick = PlayerInput.actions["RightClick"];
        _skipCutscene = PlayerInput.actions["SkipCutscene"];
        _inventoryButton = PlayerInput.actions["InventoryButton"];
        _mousePosition = PlayerInput.actions["MousePosition"];
    }


    public string GetCurrentActionMap()
    {
        return PlayerInput.currentActionMap.name;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), PlayerInput.currentActionMap.name);
    }




}
