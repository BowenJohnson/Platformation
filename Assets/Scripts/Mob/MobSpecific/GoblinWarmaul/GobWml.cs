using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Goblin Warmaul Class
public class GobWml : Entity
{
    // state declarations for states so that they can be transitioned into
    public GobWml_IdleState idleState { get; private set; }
    public GobWml_MoveState moveState { get; private set; }
    public GobWml_PlayerDetectedState playerDetectedState { get; private set; }
    public GobWml_ChargeState chargeState { get; private set; }
    public GobWml_LookForPlayerState lookForPlayerState { get; private set; }
    public GobWml_MeleeAttackState meleeAttackState { get; private set; }
    public GobWml_StunState stunState { get; private set; }
    public GobWml_DeadState deadState { get; private set; }

    [Header("Attack Transforms")]
    [SerializeField] private Transform _meleeAttackPos;

    // the variable data for each of the states
    // make serialized so they can be changed in unity
    [Header("AI State Data")]
    [SerializeField] private Data_IdleState _idleStateData;
    [SerializeField] private Data_MoveState _moveStateData;
    [SerializeField] private Data_PlayerDetected _playerDetectedData;
    [SerializeField] private Data_ChargeState _chargeStateData;
    [SerializeField] private Data_LookForPlayerState _lookForPlayerStateData;
    [SerializeField] private Data_MeleeAttack _meleeAttackStateData;
    [SerializeField] private Data_StunState _stunStateData;
    [SerializeField] private Data_DeadState _deadStateData;

    // reference to the SFX controller
    private BasicMobSFX _sfx;

    public override void Start()
    {
        // fire up the start() from entity
        base.Start();

        // instantiate each of the possible states
        idleState = new GobWml_IdleState(this, stateMachine, "idle", _idleStateData, this);
        moveState = new GobWml_MoveState(this, stateMachine, "move", _moveStateData, this);
        playerDetectedState = new GobWml_PlayerDetectedState(this, stateMachine, "playerDetected", _playerDetectedData, this);
        chargeState = new GobWml_ChargeState(this, stateMachine, "charge", _chargeStateData, this);
        lookForPlayerState = new GobWml_LookForPlayerState(this, stateMachine, "lookForPlayer", _lookForPlayerStateData, this);
        meleeAttackState = new GobWml_MeleeAttackState(this, stateMachine, "meleeAttack", _meleeAttackPos, _meleeAttackStateData, this);
        stunState = new GobWml_StunState(this, stateMachine, "stun", _stunStateData, this);
        deadState = new GobWml_DeadState(this, stateMachine, "dead", _deadStateData, this);

        _sfx = GetComponent<BasicMobSFX>();

        stateMachine.Initialize(moveState);
    }

    // draw circle for melee attack position in unity
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(_meleeAttackPos.position, _meleeAttackStateData.attackRadius);
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
        // if stunned and not already in stun state then get stunned
        else if (_isStunned && stateMachine.currState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
        // else if hit from behind turn around and look for player
        else if (!CheckMinAgro())
        {
            lookForPlayerState.SetFlipNow(true);
            stateMachine.ChangeState(lookForPlayerState);
        }
    }
    // play what input sound effect
    public void PlaySFX(AudioClip sfx)
    {
        _sfx.PlaySound(sfx);
    }

    public IEnumerator DeathTimer(float time)
    {
        // wait timer so animation can go before destroying mob
        yield return new WaitForSeconds(time);
    }
}
