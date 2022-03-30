using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controls the special attack build-up sounds and behavior
public class SpecialBuildUp : MonoBehaviour
{
    [SerializeField] private AudioClip _sfx;
    private AudioSource _audio;
    private float _destroyTime;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _destroyTime = 2f;
    }

    // Start is called before the first frame update
    void Start()
    {
        _audio.PlayOneShot(_sfx);
    }

    // called by animator to start destroy
    public void DestroyObject()
    {
        StartCoroutine(DelayedDestroy());
    }

    // disable sprite then destroys the game object after an input delay time
    private IEnumerator DelayedDestroy()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(_destroyTime);
        Destroy(transform.parent.gameObject);
    }
}
