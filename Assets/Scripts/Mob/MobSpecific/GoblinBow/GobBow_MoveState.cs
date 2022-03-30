using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobBow_MoveState : MoveState
{
    private GobBow _gobBow;

    public GobBow_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_MoveState stateData, GobBow gobBow) 
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

        // if player is in min aggro range switch to detected state
        if (_isInMinAggroRng)
        {
            _stateMachine.ChangeState(_gobBow.playerDetectedState);
        }
        // if there is a wall or no ledge to stand on idle
        else if (_isWall || !_isLedge)
        {
            _gobBow.idleState.SetFlipAfterIdle(true);
            _stateMachine.ChangeState(_gobBow.idleState);
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
