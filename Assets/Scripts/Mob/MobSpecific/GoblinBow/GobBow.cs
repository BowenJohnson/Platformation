using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobBow : Entity
{
    public GobBow_MoveState moveState { get; private set; }
    public GobBow_IdleState idleState { get; private set; }
    public GobBow_PlayerDetectedState playerDetectedState { get; private set; }
    public GobBow_MeleeAttackState meleeAttackState { get; private set; }
    public GobBow_LookForPlayerState lookForPlayerState { get; private set; }
    public GobBow_StunState stunState { get; private set; }
    public GobBow_DodgeState dodgeState { get; private set; }
    public GobBow_RangedAttackState rangedAttackState { get; private set; }
    public GobBow_DeadState deadState { get; private set; }

    [Header("Attack Transforms")]
    [SerializeField] private Transform _meleeAttackPos;
    [SerializeField] private Transform _rangedAttackPos;

    [Header("AI State Data")]
    [SerializeField] private Data_IdleState _idleStateData;
    [SerializeField] private Data_MoveState _moveStateData;
    [SerializeField] private Data_PlayerDetected _playerDetectedStateData;
    [SerializeField] private Data_MeleeAttack _meleeAttackStateData;
    [SerializeField] private Data_LookForPlayerState _lookForPlayerStateData;
    [SerializeField] private Data_StunState _stunStateData;
    [SerializeField] private Data_RangedAttackState _rangedAttackStateData;
    [SerializeField] private Data_DeadState _deadStateData;

    // public access to the dodge timer in other states
    [SerializeField] public Data_DodgeState _dodgeStateData;

    private BasicMobSFX _sfx;

    public override void Start()
    {
        base.Start();

        idleState = new GobBow_IdleState(this, stateMachine, "idle", _idleStateData, this);
        moveState = new GobBow_MoveState(this, stateMachine, "move", _moveStateData, this);
        playerDetectedState = new GobBow_PlayerDetectedState(this, stateMachine, "playerDetected", _playerDetectedStateData, this);
        meleeAttackState = new GobBow_MeleeAttackState(this, stateMachine, "meleeAttack", _meleeAttackPos, _meleeAttackStateData, this);
        lookForPlayerState = new GobBow_LookForPlayerState(this, stateMachine, "lookForPlayer", _lookForPlayerStateData, this);
        stunState = new GobBow_StunState(this, stateMachine, "stun", _stunStateData, this);
        dodgeState = new GobBow_DodgeState(this, stateMachine, "dodge", _dodgeStateData, this);
        rangedAttackState = new GobBow_RangedAttackState(this, stateMachine, "rangedAttack", _rangedAttackPos, _rangedAttackStateData, this);
        deadState = new GobBow_DeadState(this, stateMachine, "dead", _deadStateData, this);

        _sfx = GetComponent<BasicMobSFX>();

        stateMachine.Initialize(moveState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        // if dead change to dead state
        if (_isDead)
        {
            // add one to the kill counter
            FindObjectOfType<HeroController>().AddMobsKilled();

            stateMachine.ChangeState(deadState);
        }
        // else if stunned and not in stunState change to stun state
        else if (_isStunned && stateMachine.currState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
        // if player is in min aggro range change to ranged attack state
        else if (CheckMinAgro())
        {
            stateMachine.ChangeState(rangedAttackState);
        }
        // else if hit from behind turn around and look for player
        else if (!CheckMinAgro())
        {
            lookForPlayerState.SetFlipNow(true);
            stateMachine.ChangeState(lookForPlayerState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(_meleeAttackPos.position, _meleeAttackStateData.attackRadius);
    }

    public void PlaySFX(AudioClip sfx)
    {
        _sfx.PlaySound(sfx);
    }
}
