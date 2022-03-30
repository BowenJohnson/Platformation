using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this will contain all the details of the attack
// damage this will work as the go between for
// sending data between monster and player
public struct AttackDetails
{
    public Vector2 position;
    public float damage;
    public float stunDmg;
}
