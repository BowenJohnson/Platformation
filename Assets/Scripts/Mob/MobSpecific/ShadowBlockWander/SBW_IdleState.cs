using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBW_IdleState : IdleState
{
    protected SBW _sbw;

    public SBW_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_IdleState stateData, SBW sbw) 
        : base(entity, stateMachine, animBoolName, stateData)
    {
        _sbw = sbw;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_endIdle)
        {
            _stateMachine.ChangeState(_sbw.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
