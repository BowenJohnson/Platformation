using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newIdleStateData", menuName = "Data/State Data/Idle State")]

public class Data_IdleState : ScriptableObject
{
    // num of seconds range to idle
    public float minIdle = 1f;
    public float maxIdle = 2f;

    // in case some mobs make a noise while idling
    [SerializeField] public AudioClip _idleAudio;
}
