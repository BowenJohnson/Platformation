using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSFX : MonoBehaviour
{
    private AudioSource _audio;
    [SerializeField] AudioClip[] _sfxClips;


    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // plays the sound that is currently cached in the unity editor
    public void PlaySound()
    {
        _audio.Play();
    }

    // plays a random sound from an array of sounds
    public void PlayRandomSound()
    {
        // set clip to a random sound effect from the array then play it
        AudioClip clip = _sfxClips[UnityEngine.Random.Range(0, _sfxClips.Length)];
        _audio.PlayOneShot(clip);
    }
}
