using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] private Color originalButtonColor;
    private Color changedButtonColor;
    private TextMeshProUGUI buttonText;
    private Button _button;

    [SerializeField] private float transitionDuration = 0.2f;
    private IEnumerator LerpCoroutine;

    

    void Start()
    {
        changedButtonColor = DialogueManager.GetInstance().currentColor;
        buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ReturnColor()
    {
        if(LerpCoroutine != null)
        {
            StopCoroutine(LerpCoroutine);
        }
        LerpCoroutine = LerpColor(buttonText.color, originalButtonColor);
        StartCoroutine(LerpCoroutine);
    }

    public void HoverColor()
    {
        if(LerpCoroutine != null)
        {
            StopCoroutine(LerpCoroutine);
        }
        LerpCoroutine = LerpColor(originalButtonColor, changedButtonColor);
        StartCoroutine(LerpCoroutine);
    }

    IEnumerator LerpColor(Color startColor, Color endColor)
    {
        float elapsedTime = 0;
        float lerpProgress = 0;
        Color newColor = new Color();

        while(lerpProgress < 1)
        {
            elapsedTime += Time.deltaTime;
            lerpProgress = elapsedTime/transitionDuration;
            newColor = Color.Lerp(startColor, endColor, lerpProgress);
            buttonText.color = newColor;
            
            yield return new WaitForEndOfFrame();

        }
    }
}   
