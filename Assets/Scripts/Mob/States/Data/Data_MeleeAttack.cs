using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/State Data/Melee Attack State")]
public class Data_MeleeAttack : ScriptableObject
{
    // size of the damage area, amount of damage dealt
    // and what layer should be hit
    public float attackRadius = 0.2f;
    public float attackDmg = 2;
    public LayerMask whatIsPlayer;

    // melee attack SFX
    [SerializeField] public AudioClip _meleeSwingSFX;
    [SerializeField] public AudioClip _meleeHitSFX;
    [SerializeField] public AudioClip _meleeHitWallSFX;

    // additional hit particle effect slot
    [SerializeField] public GameObject _hitParticle;
}
