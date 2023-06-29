using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
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

    [SerializeField] private GameObject DialogueUI;
    [SerializeField] private GameObject InteractableOverlayUI;

    public static DialogueManager GetInstance()
    {
        return instance;
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
