using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]

// storing all variables related to Entity
public class Data_Entity : ScriptableObject
{
    // hp and knock up speed when hit
    public float maxHealth = 3.0f;
    public float damageBounce = 1.0f;

    // number of times mob needs to get hit to be stunned
    public float stunResistPoints = 2.0f;
    
    // time between hits to restart stun hit counter
    public float stunRecoverTime = 2.0f;

    // wall and edge check raycast ranges
    public float wallCheckDist = 0.2f;
    public float ledgeCheckDist = 0.4f;
    public float groundCheckRadi = 0.2f;

    // aggro distances for detection and evasion
    public float minAggroDist = 2.0f;
    public float maxAggroDist = 3.0f;

    // range for starting close range action
    public float closeRngActDist = 0.38f;

    // add hit particles game object here
    
    // layer masks for targeting
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;
}
