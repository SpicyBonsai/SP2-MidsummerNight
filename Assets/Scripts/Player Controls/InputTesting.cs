using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTesting : MonoBehaviour
{
    public PlayerInput playerInput;
    public InputAction fireAction;
    private InputAction switchContext;

    
    //private bool fire;
    void Awake() 
    {
        playerInput = GetComponent<PlayerInput>();
        fireAction = playerInput.currentActionMap.FindAction("Fire");
        switchContext = playerInput.currentActionMap.FindAction("SwitchContext");
        playerInput.onActionTriggered += ReadAction;
    }


    private void ReadAction(InputAction.CallbackContext context)
    {
        if(context.action == fireAction)
        {
            if(context.started) 
            {
                Debug.Log(playerInput.currentActionMap);
                //fire = true;
            }
            if(context.canceled) 
            {
                Debug.Log("Mouse Declicked");
                //fire = false;
            }
        }

        if(context.action == switchContext)
        {
            
            Debug.Log(context.action.GetBindingIndex());
        }
    }

}

    // public InputAction fire;
    // [SerializeField] private InputActionAsset controls;
    // private InputActionMap _inputActionMap;

    // void Start()
    // {
    //     // _inputActionMap = controls.FindActionMap("Gameplay");   
    //     // fire = _inputActionMap.FindAction("Fire");
    //     // fire.performed += OnFireAction;
    //     // Debug.Log("start called");
    // }

    // private void OnFireAction(InputAction.CallbackContext obj)
    // {
    //     Debug.Log("Fired!");
    // }

    
    // void Update()
    // {
        
    // }