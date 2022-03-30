using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobWml_PlayerDetectedState : PlayerDetectedState
{
    private GobWml _gobWml;

    public GobWml_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_PlayerDetected stateData, GobWml gobWml) 
        : base(entity, stateMachine, animBoolName, stateData)
    {
        _gobWml = gobWml;
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
            _stateMachine.ChangeState(_gobWml.meleeAttackState);
        }
        // if long range action is true then change
        // to charge state
        else if (_doLongRngAction)
        {
            _stateMachine.ChangeState(_gobWml.chargeState);
        }
        // if player gets out of max aggro range start looking for player
        else if (!_isInMaxAggroRng)
        {
            _stateMachine.ChangeState(_gobWml.lookForPlayerState);
        }
        // if mob comes to an edge while player is detected then turn around and walk away
        else if (!_isLedge)
        {
            _entity.Flip();
            _stateMachine.ChangeState(_gobWml.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
