using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassageFix : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] [ColorUsage(true,true)] private Color blueFlames;
    [SerializeField] [ColorUsage(true,true)] private Color blueFlamesOff;
    [SerializeField] private GameObject teleporter;
    [SerializeField] private MeshRenderer floor;
    [SerializeField] private float desiredDuration = 3f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Animate()
    {
        _animator.SetTrigger("PutInPlace");
    }

    public void RevealItems()
    {
        Debug.Log("Hello");
        AudioManager.Instance.RandomSoundEffect(AudioManager.Instance.restoreGate);
        StartCoroutine(StartReveal());
    }

    private IEnumerator StartReveal()
    {
        yield return new WaitForSeconds(1.05f);
        StartCoroutine(Reveal(floor));
        StartCoroutine(Reveal(teleporter.GetComponent<MeshRenderer>()));
    }

    private IEnumerator Reveal(MeshRenderer meshRenderer)
    {
        teleporter.SetActive(true);
        float elapsedTime = 0f,
            lerpProcess = 0f;

        while (lerpProcess < 1f)
        {
            elapsedTime += Time.deltaTime;
            lerpProcess = elapsedTime / desiredDuration;
            
            meshRenderer.material.SetColor("_EmissionColor", Color.Lerp(blueFlamesOff, blueFlames, lerpProcess));
            yield return new WaitForEndOfFrame();
        }
    }
}
