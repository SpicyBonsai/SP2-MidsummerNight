using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    [SerializeField] private AudioSource _musicBox;
    private int _songListSize;
    private int _lastSongIndex;
    private void Start()
    {
        _musicBox = gameObject.GetComponent<AudioSource>();
        _songListSize = Random.Range(0, AudioManager.Instance.songs.Length);
        _lastSongIndex = _songListSize + 1;
    }
    
    private void Update()
    {
        if (!_musicBox.isPlaying && !_musicBox.mute)
        {
            PlaySong();
        }

        void PlaySong()
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, _songListSize);
                
            } while (randomIndex == _lastSongIndex);
            _lastSongIndex = randomIndex;
            
            _musicBox.PlayOneShot(AudioManager.Instance.songs[randomIndex].audioClip);
        }
    }
}
