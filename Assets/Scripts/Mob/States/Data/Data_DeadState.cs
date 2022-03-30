using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDeadStateData", menuName = "Data/State Data/Dead State")]
public class Data_DeadState : ScriptableObject
{
    // can use this area to keep track of any blood particals and etc.
    // if they are needed for any mobs that use the dead state
    [SerializeField] public Transform _hitParticle;
    [SerializeField] public Transform _deathParticle;
    [SerializeField] public float _destroyDelayTime;
    [SerializeField] public AudioClip _deadSFX;
}
