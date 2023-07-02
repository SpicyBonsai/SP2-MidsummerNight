using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject DialogueUI;
    [SerializeField] private GameObject InteractableOverlayUI;
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private RectTransform imageOffsetRect;
    [SerializeField] private Vector3 initialImagePosition = new Vector3(-900, -500, 0);
    [SerializeField] public Color currentColor { get; private set; }

    [SerializeField] private TextMeshProUGUI[] dialogueButtons;
    
    #region Singleton Pattern Setup
    private static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Input Manager in the scene.");
        }
        instance = this;
    }
    #endregion

    private void OnEnable() 
    {
   
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    public Vector2 GetImagePosition()
    {
        return imageOffsetRect.position;
    }

    public void OffsetImage(Vector2 offset)
    {
        imageOffsetRect.anchoredPosition = new Vector3(initialImagePosition.x + offset.x, initialImagePosition.y + offset.y, initialImagePosition.z);
    
    }

    public void ScaleImage(float scaleAmount)
    {
        imageOffsetRect.localScale = Vector3.one * scaleAmount;
    }

    public void SetImage(Sprite image)
    {
        if (image == null)
        {
            profileImage.color = Color.clear;
        } 
        else
        {
            profileImage.color = Color.white;
        }

        profileImage.sprite = image;
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetCurrentColor(Color color)
    {
        currentColor = color;
    }

    public void SetNameColor(Color color)
    {
        nameText.color = color;
    }

    public void SetButtonsColor(Color color)
    {
        foreach(TextMeshProUGUI button in dialogueButtons)
        {
            button.color = color;
        }
    }

    public void OpenDialogue()
    {
        DialogueUI.SetActive(true);
    }

    public GameObject GetDialogueUI()
    {
        return DialogueUI;
    }

    public void CloseDialogue()
    {
        DialogueUI.SetActive(false);
    }

    public void SetInteractableOverlay(bool value)
    {
        InteractableOverlayUI.SetActive(value);
    }

}
