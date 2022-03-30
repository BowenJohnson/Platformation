using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBoss_BackhandAttackState : MeleeAttackState
{
    private DemonBoss _demonBoss;

    protected bool _isInMaxAggroRng;

    public DemonBoss_BackhandAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attkPos, Data_MeleeAttack stateData, DemonBoss demonBoss)
        : base(entity, stateMachine, animBoolName, attkPos, stateData)
    {
        _demonBoss = demonBoss;
    }

    public override void Enter()
    {
        base.Enter();
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
    }
}
