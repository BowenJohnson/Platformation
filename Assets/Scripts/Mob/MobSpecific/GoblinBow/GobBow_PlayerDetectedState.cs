using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobBow_PlayerDetectedState : PlayerDetectedState
{
    private GobBow _gobBow;

    public GobBow_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_PlayerDetected stateData, GobBow gobBow) 
        : base(entity, stateMachine, animBoolName, stateData)
    {
        _gobBow = gobBow;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // if can do close range action switch to melee attack state
        if (_doCloseRngAction)
        {
            // if dodge cool down timer is over then dodge otherwise melee attack
            if (Time.time >= _gobBow.dodgeState._startTime + _gobBow._dodgeStateData.dodgeCoolDwn)
            {
                _stateMachine.ChangeState(_gobBow.dodgeState);
            }
            else
            {
                _stateMachine.ChangeState(_gobBow.meleeAttackState);
            }
        }
        // if in long range action range switch to ranged attack state
        else if (_doLongRngAction)
        {
            _stateMachine.ChangeState(_gobBow.rangedAttackState);
        }
        else if (!_isInMaxAggroRng)
        {
            _stateMachine.ChangeState(_gobBow.lookForPlayerState);
        }
    }

    public override void MakeChecks()
    {
        base.MakeChecks();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
