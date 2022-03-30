using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobBow_IdleState : IdleState
{
    private GobBow _gobBow;

    public GobBow_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_IdleState stateData, GobBow gobBow) 
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

        // if player is in min aggro range switch to player detected state
        if (_isInMinAggroRng)
        {
            _stateMachine.ChangeState(_gobBow.playerDetectedState);
        }
        // else start moving again
        else if (_endIdle)
        {
            _stateMachine.ChangeState(_gobBow.moveState);
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
