using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBW_MoveState : MoveState
{
    protected SBW _sbw;

    public SBW_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_MoveState stateData, SBW sbw) 
        : base(entity, stateMachine, animBoolName, stateData)
    {
        _sbw = sbw;
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

        // if hit a wall or ledge hit an idle state and flip after
        if (_isWall || !_isLedge)
        {
            _sbw.idleState.SetFlipAfterIdle(true);
            _stateMachine.ChangeState(_sbw.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // keep moving in an attempt to bump the player
        _entity.SetVelocity(_stateData.movementSpeed);
    }
}
