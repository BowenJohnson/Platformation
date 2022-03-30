using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class holds and controls player or other entity sound effects
public class EntitySFX : MonoBehaviour
{
    // dictionary to hold the clips
    private Dictionary<string, AudioClip> _sfxClips;

    // reference to the audio player
    private AudioSource _audio;

    // temp var to hold retrieved clips to be played
    private AudioClip _tempClip;

    // the clips to be added
    [SerializeField] private AudioClip _basicAttack;
    [SerializeField] private AudioClip _jump;
    [SerializeField] private AudioClip _land;
    [SerializeField] private AudioClip _death;
    [SerializeField] private AudioClip _special;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _sfxClips = new Dictionary<string, AudioClip>();
        AddClips();
    }

    // adds all the clips to the dictionary
    public void AddClips()
    {
        _sfxClips.Add("basicAttack", _basicAttack);
        _sfxClips.Add("jump", _jump);
        _sfxClips.Add("land", _land);
        _sfxClips.Add("death", _death);
        _sfxClips.Add("special", _special);
    }

    // plays the clip based on the name passed in via string
    public void PlayClip(string clipName)
    {
        // save the clip from the dictionary to temp var
        _tempClip = _sfxClips[clipName];

        // play the retrieved clip if it isn't NULL
        if (_tempClip)
        {
            _audio.PlayOneShot(_tempClip);
        }
    }    
}
