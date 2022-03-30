using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSFX : MonoBehaviour
{
    private AudioSource _audio;
    [SerializeField] AudioClip _shoot;
    [SerializeField] AudioClip _hitEntity;
    [SerializeField] AudioClip _hitWall;

    // call this in awake so I don't get a null reference
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void PlayShoot()
    {
        _audio.PlayOneShot(_shoot);
    }

    public void PlayHitEntity()
    {
        _audio.PlayOneShot(_hitEntity);
    }

    public void PlayHitWall()
    {
        _audio.PlayOneShot(_hitWall);
    }
}
