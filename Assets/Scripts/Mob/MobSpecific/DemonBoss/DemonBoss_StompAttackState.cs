using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBoss_StompAttackState : MeleeAttackState
{
    private DemonBoss _demonBoss;

    private bool _isInMaxAggroRng;

    private Transform _stompFlameSpawn;
    private Transform _frontFireballSpawn;
    private Transform _backFireballSpawn;

    // projectile object and stats
    private GameObject _fireballPrefab;
    private GameObject _fireball1;
    private GameObject _fireball2;
    private float _rngSpeed;
    private float _rngDist;
    private float _rngDmg;

    public DemonBoss_StompAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attkPos, 
        Data_MeleeAttack stateData, DemonBoss demonBoss, GameObject fireballPrefab, Transform stompFlameSpawn, Transform frontFireballSpawn, 
        Transform backFireballSpawn, float rngSpeed, float rngDist, float rngDmg)
        : base(entity, stateMachine, animBoolName, attkPos, stateData)
    {
        _demonBoss = demonBoss;
        _stompFlameSpawn = attkPos;
        _fireballPrefab = fireballPrefab;
        _frontFireballSpawn = frontFireballSpawn;
        _backFireballSpawn = backFireballSpawn;
        _rngSpeed = rngSpeed;
        _rngDist = rngDist;
        _rngDmg = rngDmg;
    }

    public override void Enter()
    {
        base.Enter();

        // reset special attack counter
        _demonBoss.ResetSpecialHits();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // if attack animation is done check player range
        if (_isAnimDone)
        {
            // if player is in min aggro range then
            // change to player detected state
            if (_isInMinAggroRng)
            {
                _stateMachine.ChangeState(_demonBoss.playerDetectedState);
            }
            // else if player is still in front of mob start walking
            else if (_isInMaxAggroRng)
            {
                _stateMachine.ChangeState(_demonBoss.moveState);
            }
            // else the player is behind mob, idle and turn around
            else
            {
                _demonBoss.idleState.SetFlipAfterIdle(true);
                _stateMachine.ChangeState(_demonBoss.idleState);
            }
        }
    }

    public override void MakeChecks()
    {
        base.MakeChecks();

        _isInMaxAggroRng = _entity.CheckMaxAgro();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void StartAttack()
    {
        base.StartAttack();

        // instantiate ground chunks on attack position using its state data 
        Object.Instantiate(_stateData._hitParticle, _stompFlameSpawn.position, _stateData._hitParticle.transform.rotation);

        // instantiate fireball1
        // instantiate projectile object on attack position using its state data 
        _fireball1 = Object.Instantiate(_fireballPrefab, _frontFireballSpawn.position, _frontFireballSpawn.rotation);

        // call fire function from script
        _fireball1.GetComponent<MagicAttack>().FireProjectile(_rngSpeed, _rngDist, _rngDmg);

        // instantiate fireball2
        // instantiate projectile object on attack position using its state data 
        _fireball2 = Object.Instantiate(_fireballPrefab, _backFireballSpawn.position, _backFireballSpawn.rotation);

        // call fire function from script
        _fireball2.GetComponent<MagicAttack>().FireProjectile(_rngSpeed, _rngDist, _rngDmg);
    }
}
