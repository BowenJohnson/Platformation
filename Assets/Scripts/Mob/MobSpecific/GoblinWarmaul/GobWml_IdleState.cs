using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobWml_IdleState : IdleState
{
    private GobWml _gobWml;

    public GobWml_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_IdleState stateData, GobWml gobWml) 
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

        // if player is in min aggro range switch to player detected state
        if (_isInMinAggroRng)
        {
            _stateMachine.ChangeState(_gobWml.playerDetectedState);
        }
        // else start moving again
        else if (_endIdle)
        {
            _stateMachine.ChangeState(_gobWml.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
