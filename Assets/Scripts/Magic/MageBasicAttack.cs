using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class controls the mage hero basic attack prefab
public class MageBasicAttack : MonoBehaviour
{
    private AudioSource _audio;
    [SerializeField] private AudioClip _sfx;

    private float _destroyTime = .5f;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.PlayOneShot(_sfx);       
    }

    // called by animator to start destroy
    public void DestroyBasicAttack()
    {
        StartCoroutine(DelayedDestroy());
    }

    // disable sprite then destroys the game object after an input delay time
    protected IEnumerator DelayedDestroy()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(_destroyTime);
        Destroy(gameObject);
    }
}
