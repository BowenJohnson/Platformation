using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBoss_WaitState : IdleState
{
    private DemonBoss _demonBoss;

    public DemonBoss_WaitState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_IdleState stateData, DemonBoss demonBoss) 
        : base(entity, stateMachine, animBoolName, stateData)
    {
        _demonBoss = demonBoss;
    }

    public override void Enter()
    {
        base.Enter();
        _entity.SetVelocity(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_demonBoss.waitOver)
        {
            _stateMachine.ChangeState(_demonBoss.idleState);
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
