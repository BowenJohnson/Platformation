using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSFX : MonoBehaviour
{
    private AudioSource _audio;
    private AudioClip _currBuildup;
    [SerializeField] AudioClip[] _buildupSFX;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // set current buildup SFX clip from clips array
    public void SetBuildupClip(int sfxNum)
    {
        _currBuildup = _buildupSFX[sfxNum];
    }

    // set current buildup SFX clip to null
    public void SetBuildupClip()
    {
        _currBuildup = null;
    }

    public void PlayCurrentSFX()
    {
        _audio.PlayOneShot(_currBuildup);
    }
}
