using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobWml_MoveState : MoveState
{
    private GobWml _gobWml;

    public GobWml_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_MoveState stateData, GobWml gobWml) 
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
        // if hit a wall or ledge hit an idle state and flip after
        else if (_isWall || !_isLedge)
        {
            _gobWml.idleState.SetFlipAfterIdle(true);
            _stateMachine.ChangeState(_gobWml.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
