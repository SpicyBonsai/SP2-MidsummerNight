using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubtitleOnEnter : MonoBehaviour
{
    // string[] words = new string[0];
    TextMeshProUGUI textMesh;
    [SerializeField] float desiredDuration = 1f;

    Color startColor = new Color(1,1,1,0);
    Color endColor = new Color(1,1,1,1);

    private void Awake() 
    {
        textMesh = gameObject.GetComponent<TextMeshProUGUI>();
        StartCoroutine(AnimateIntro());

        // words = textMesh.text.Split(' ');
        // StartCoroutine(AnimateWords());
    }

    IEnumerator AnimateIntro()
    {
        float elapsedTime = 0;
        float lerpTime = 0;
        textMesh.color = startColor;

        while(textMesh.color.a < 1 && !InputManager.GetInstance().Submit)
        {
            elapsedTime += Time.deltaTime;
            lerpTime = elapsedTime / desiredDuration;
            textMesh.color = Color.Lerp(startColor, endColor, lerpTime * lerpTime * lerpTime);
            yield return new WaitForEndOfFrame();
        }
    }

    // IEnumerator AnimateWords()
    // {
    //     string newString = "";
    //     foreach(string word in words)
    //     {
    //         newString += word + " ";
    //         textMesh.text = newString;

    //         yield return new WaitForSeconds(1f);
    //     }
    // }

}

