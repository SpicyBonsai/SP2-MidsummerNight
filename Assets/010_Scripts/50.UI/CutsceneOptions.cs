using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CutsceneOptions : MonoBehaviour
{
    [SerializeField] PlayableDirector _director;
    Coroutine _skipCoroutine;

    [SerializeField] GameObject _cutscenePlayer;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _scriptedObjects;
    [SerializeField] GameObject _skippingIndicator;
    [SerializeField] Image _skippingImage;
    private TextMeshProUGUI _skippingText;
    private float _timeToSkip = 5f;


    [SerializeField] GameOptions _gameOptions;
    [SerializeField] GameObject _subtitles;

    private void Start() {
        
        if(!_gameOptions.SubtitlesOn)
        {
            _subtitles.SetActive(false);
            foreach(var binding in _director.playableAsset.outputs.ToArray())
            {
                if( binding.streamName.Contains("Sub"))
                {
                    _director.SetGenericBinding(binding.sourceObject, null);
                }
            }

        }
        _skippingText = _skippingIndicator.GetComponentInChildren<TextMeshProUGUI>();
    }
    void Update()
    {
        if(InputManager.GetInstance().SkipCutscene)
        {
            if(_skipCoroutine == null)
            {
                Debug.Log("Started Skipping");
                _skipCoroutine = StartCoroutine(skipCoroutine());
                _skippingIndicator.SetActive(true);
            }
        }

        if(InputManager.GetInstance().CancelSkipCutscene)
        {
            Debug.Log("Cancelled Skipping");
            if(_skipCoroutine != null)
            {
                StopCoroutine(_skipCoroutine);
                _skipCoroutine = null;
            }
            _skippingIndicator.SetActive(false);
        }
    }

    private IEnumerator skipCoroutine()
    {
        float elapsedTime = 0;
        float progress = 0;
        while(progress < 1f && !InputManager.GetInstance().LeftClick)
        {
            progress = elapsedTime/_timeToSkip;
            if(progress > 0.2f && progress < 0.5f)
            {
                _skippingText.text = "Skipping...";
            }
            else if(progress > 0.5f && progress < 0.8f)
            {
                _skippingText.text = "Shame on you!";
            }
            else if(progress > 0.8f)
            {
                _skippingText.text = "You Monster!";
            }
            else
            {
                _skippingText.text = "Skip Cutscene";
            }

            _skippingImage.fillAmount = progress;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _director.Stop();
        EndCutscene();
    }

    public void EndCutscene()
    {
        _cutscenePlayer.SetActive(false);
        _player.SetActive(true);
        _scriptedObjects.SetActive(true);
    }
}
