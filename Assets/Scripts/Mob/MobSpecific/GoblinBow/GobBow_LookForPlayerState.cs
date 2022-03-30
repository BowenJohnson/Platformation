using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobBow_LookForPlayerState : LookForPlayerState
{
    private GobBow _gobBow;

    public GobBow_LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_LookForPlayerState stateData, GobBow gobBow) 
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

        if (_isInMinAggroRng)
        {
            _stateMachine.ChangeState(_gobBow.playerDetectedState);
        }
        else if (_isTurnTimeOver)
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
