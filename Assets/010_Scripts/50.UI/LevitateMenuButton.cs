using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevitateMenuButton : MonoBehaviour
{
    private RectTransform _rectTransform;
    [SerializeField] private float _levitateDistance = 0.1f;
    [SerializeField] private float _levitateSpeed = 1f;
    
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    
    void Update()
    {
        _rectTransform.anchoredPosition += new Vector2(0, Mathf.Sin(Time.time * _levitateSpeed) * _levitateDistance);
        // Debug.Log(Mathf.Sin(Time.time));
    }
}
