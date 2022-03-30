using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRangedAttackStateData", menuName = "Data/State Data/Ranged Attack State")]

public class Data_RangedAttackState : ScriptableObject
{
    // game obj to store the instantiated projectile
    public GameObject projectile;

    // damage and speed vars
    public float projDmg = 2.0f;
    public float projSpeed = 5.0f;

    // travel distance before it gets affected by gravity
    public float projTravDist = 8.0f;

    // ranged attack SFX
    [SerializeField] public AudioClip _rangedAttackSFX;
}
