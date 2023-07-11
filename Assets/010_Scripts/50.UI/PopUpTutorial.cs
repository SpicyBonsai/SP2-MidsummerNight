using System;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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
    [SerializeField] private Color popUpColor;
    private MeshRenderer _meshRenderer;
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _menuOpen = false;
    }

    void Update()
    {
        _meshRenderer.material.SetColor("_EmissionColor", popUpColor * (Mathf.Abs(Mathf.Sin(Time.time)) + 0.5f)); 
        
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
