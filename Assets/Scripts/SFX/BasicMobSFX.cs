using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class controls the Audio source for the basic mobs
public class BasicMobSFX : MonoBehaviour
{
    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    // play what ever sound effect is passed in
    // the sounds will be stored in mob state data files
    public void PlaySound(AudioClip sfx)
    {
        _audio.PlayOneShot(sfx);
    }
}
