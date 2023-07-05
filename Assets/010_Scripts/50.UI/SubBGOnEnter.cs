using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubBGOnEnter : MonoBehaviour
{
    Image image;
    Color startColor = new Color(0, 0, 0, 0);
    Color endColor = new Color(0, 0, 0, 0.6f);
    [SerializeField] float desiredDuration = 3f;

    void Awake()
    {
        image = gameObject.GetComponent<Image>();
        StartCoroutine(AnimateIntro());
    }

    
    IEnumerator AnimateIntro()
    {
        float elapsedTime = 0;
        float lerpTime = 0;
        image.color = startColor;

        while(image.color.a < endColor.a && !InputManager.GetInstance().Submit)
        {
            elapsedTime += Time.deltaTime;
            lerpTime = elapsedTime / desiredDuration;
            image.color = Color.Lerp(startColor, endColor, lerpTime * lerpTime * lerpTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
