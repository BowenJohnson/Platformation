using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newStunStateData", menuName = "Data/State Data/Stun State")]
public class Data_StunState : ScriptableObject
{
    // amount of time mob is stunned for
    public float stunnedTime = 2f;

    // enemy knockback duration, speed, direction
    public float knockbackTime = 0.2f;
    public float knockbackSpeed = 2.0f;
    public Vector2 knockbackDir;
}
