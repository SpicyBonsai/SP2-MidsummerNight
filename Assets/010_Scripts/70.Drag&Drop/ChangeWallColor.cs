using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWallColor : MonoBehaviour, IColorChanger
{

    private MeshRenderer _meshRenderer;
    [SerializeField] private float desiredDuration;
    [SerializeField] private Color startColor = new Color(0, 0, 0, 0);
    [SerializeField] private Color endColor = new Color(0, 0, 0, 1);
    private Coroutine _colorChange;
    private void Start()
    {
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        
    }

    public void ChangeColor()
    {
        if (_colorChange == null)
        {
            _colorChange = StartCoroutine(Reveal(startColor, endColor));
        }
    }
    
    private IEnumerator Reveal(Color startColor, Color endColor)
    {
        float elapsedTime = 0f,
            lerpProcess = 0f;

        while (lerpProcess < 1f)
        {
            elapsedTime += Time.deltaTime;
            lerpProcess = elapsedTime / desiredDuration;
            
            //_meshRenderer.material.color = Color.Lerp(startColor, endColor, lerpProcess);
            foreach (Material material in _meshRenderer.materials)
            {
                material.color = Color.Lerp(startColor, endColor, lerpProcess);
            }
            yield return new WaitForEndOfFrame();
        }

        _colorChange = null;
    }
}
