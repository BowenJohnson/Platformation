using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// shadowblock wanderer entity class
public class SBW : Entity
{
    public SBW_IdleState idleState { get; private set; }
    public SBW_MoveState moveState { get; private set; }
    public SBW_DeadState deadState { get; private set; }

    [Header("AI State Data")]
    [SerializeField] private Data_IdleState idleStateData;
    [SerializeField] private Data_MoveState moveStateData;
    [SerializeField] private Data_DeadState _deadStateData;

    private BasicMobSFX _sfx;

    public override void Start()
    {
        // fire up the start() from entity
        base.Start();

        idleState = new SBW_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new SBW_MoveState(this, stateMachine, "move", moveStateData, this);
        deadState = new SBW_DeadState(this, stateMachine, "dead", _deadStateData, this);

        _sfx = GetComponent<BasicMobSFX>();

        stateMachine.Initialize(moveState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        // if dead then change to dead state
        if (_isDead)
        {
            // add one to the kill counter
            FindObjectOfType<HeroController>().AddMobsKilled();

            stateMachine.ChangeState(deadState);
        }
    }

    // destroy the game object
    public void Destroy()
    {        
        Destroy(this.gameObject);
    }

    public void PlaySFX(AudioClip sfx)
    {
        _sfx.PlaySound(sfx);
    }
}