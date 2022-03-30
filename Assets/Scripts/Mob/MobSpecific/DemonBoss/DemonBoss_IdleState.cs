using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBoss_IdleState : IdleState
{
    private DemonBoss _demonBoss;

    private bool _isInMaxAggroRng;

    public DemonBoss_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_IdleState stateData, DemonBoss demonBoss) 
        : base(entity, stateMachine, animBoolName, stateData)
    {
        _demonBoss = demonBoss;
    }

    public override void Enter()
    {
        base.Enter();

        _entity.GetComponent<AudioSource>().PlayOneShot(_stateData._idleAudio);
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
            _stateMachine.ChangeState(_demonBoss.playerDetectedState);
        }
        // else start moving again
        else if (_endIdle)
        {
            _stateMachine.ChangeState(_demonBoss.moveState);
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
