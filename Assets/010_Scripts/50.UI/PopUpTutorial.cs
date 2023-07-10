using System;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class PopUpTutorial : MonoBehaviour, IHoverable
{
    private bool _isInteracting;
    [SerializeField] private Animator animator;
    private bool _menuOpen;
    private bool _interactValue;
    void Start()
    {
        _menuOpen = false;
    }

    void Update()
    {
        //Debug.Log("Interact value: " + _interactValue);
        _interactValue = IsHovering();

        if (_menuOpen && !_interactValue)
        {
            //Debug.Log("closing menu...");
            CloseMenu();
        } 
        else if (!_menuOpen && _interactValue)
        {
            //Debug.Log("opening menu...");
            OpenMenu();    
        }
        
    }

    public void OpenMenu()
    {
        animator.SetBool("Open", true);
        _menuOpen = true;
    }

    public void CloseMenu()
    {
        animator.SetBool("Open", false);
        _menuOpen = false;
    }

    public void Interact()
    {
        _isInteracting = true;
    }

    private bool IsHovering()
    {
        if (_isInteracting)
        {
            _isInteracting = false;
            return true;
        }

        return false;
    }
}
