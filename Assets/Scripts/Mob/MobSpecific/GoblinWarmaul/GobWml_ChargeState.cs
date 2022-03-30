using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobWml_ChargeState : ChargeState
{
    private GobWml _gobWml;

    public GobWml_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_ChargeState stateData, GobWml gobWml) 
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

    // if player is still being detected
    // or not will determine new state transitions
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // if can do close range action switch to attack state
        if (_doCloseRngAction)
        {
            _stateMachine.ChangeState(_gobWml.meleeAttackState);
        }
        // if enemy comes to an edge or wall start looking for player
        else if (!_isLedge || _isWall)
        {
            _stateMachine.ChangeState(_gobWml.lookForPlayerState);
        }
        else if (_isChargeTimeEnd)
        {
            // if enemy still detects the player
            // enter player detected state
            if (_isInMinAggroRng)
            {
                _stateMachine.ChangeState(_gobWml.playerDetectedState);
            }
            else
            {
                _stateMachine.ChangeState(_gobWml.lookForPlayerState);
            }
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
