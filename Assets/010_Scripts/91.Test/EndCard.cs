using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndCard : MonoBehaviour
{
    [SerializeField] private string textToDisplay;
    [SerializeField] private TextMeshProUGUI _textMesh;
    
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        string emptyString = "";
        foreach (char character in textToDisplay)
        {
            emptyString += character;
            _textMesh.text = emptyString;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(2f);
        LevelManager.instance.ReturnToMainMenu();
    }

    void Update()
    {
        
    }
}
