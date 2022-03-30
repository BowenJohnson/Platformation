using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{

    private void Start()
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        particle.Play();
    }
}
