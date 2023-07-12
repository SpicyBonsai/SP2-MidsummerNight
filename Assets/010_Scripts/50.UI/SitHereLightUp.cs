using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitHereLightUp : MonoBehaviour
{
    private ColliderCheck _sphereCollider;
    private MeshRenderer _meshRenderer;
    [SerializeField] private float desiredDuration;
    [SerializeField] private Color startColor = new Color(0, 0, 0, 0);
    [SerializeField] private Color endColor = new Color(0, 0, 0, 1);
    private Coroutine _colorChange;
    private bool _playerInZone;
    [SerializeField] private float RotationSpeed = 10f;
    private Vector3 _rotation; 
    void Start()
    {
        _sphereCollider = gameObject.GetComponentInChildren<ColliderCheck>();
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    
    void Update()
    {
        //_rotation = Vector3.up * Time.deltaTime * RotationSpeed + gameObject.transform.localRotation;
        gameObject.transform.Rotate(Time.deltaTime * RotationSpeed * Vector3.up , Space.Self);
        if (!_playerInZone && _sphereCollider.PlayerInRange && _colorChange == null)
        {
            _playerInZone = true;
            _colorChange = StartCoroutine(Reveal(_meshRenderer, startColor, endColor));
        }
        else if(_playerInZone && !_sphereCollider.PlayerInRange && _colorChange == null)
        {
            _playerInZone = false;
            _colorChange = StartCoroutine(Reveal(_meshRenderer, endColor, startColor));
        }
    }
    
    private IEnumerator Reveal(MeshRenderer meshRenderer, Color startColor, Color endColor)
    {
        float elapsedTime = 0f,
            lerpProcess = 0f;

        while (lerpProcess < 1f)
        {
            elapsedTime += Time.deltaTime;
            lerpProcess = elapsedTime / desiredDuration;
            
            //meshRenderer.material.SetColor("_EmissionColor", Color.Lerp(startColor, endColor, lerpProcess));
            meshRenderer.material.color = Color.Lerp(startColor, endColor, lerpProcess);
            yield return new WaitForEndOfFrame();
        }

        _colorChange = null;
    }
}
