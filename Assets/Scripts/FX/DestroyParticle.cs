using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    private GameObject _particleObject;
    private float _destroyTimeDelay = 1.25f;

    void Start()
    {
        _particleObject = this.gameObject;
    }

    // function called by other class to destroy the particle
    // game object
    public void DestroyThisParticle()
    {
        StartCoroutine(Destroy(_destroyTimeDelay));
    }

    // coroutine so that particles have time to dissapate
    // before the object is destroyed to make it less abrupt
    IEnumerator Destroy(float timer)
    {
        _particleObject.GetComponentInChildren<ParticleSystem>().Stop();
        yield return new WaitForSeconds(timer);
        Destroy(_particleObject);
    }
}
