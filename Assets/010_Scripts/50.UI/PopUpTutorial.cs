using System;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class PopUpTutorial : MonoBehaviour, IPointerEnterHandler
{
    private bool _isInteracting;
    private Animator _animator;
    private bool _menuOpen;
    private bool _interactValue;
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _menuOpen = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Open the fucking menu");
        OpenMenu();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Close the fucking menu");
        CloseMenu();
    }

    void Update()
    {
        _interactValue = IsHovering();

        if (_menuOpen && !_interactValue)
        {
            CloseMenu();
        } 
        else if (_menuOpen && _interactValue)
        {
            OpenMenu();    
        }
        
    }

    public void OpenMenu()
    {
        _animator.SetBool("Open", true);
        _menuOpen = true;
    }

    public void CloseMenu()
    {
        _animator.SetBool("Open", false);
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
