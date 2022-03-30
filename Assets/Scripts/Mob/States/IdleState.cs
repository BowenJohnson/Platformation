using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected Data_IdleState _stateData;

    protected float _idleTime;

    // bools for flipping, ending idle,
    // and player in aggro range
    protected bool _flipAfterIdle;
    protected bool _endIdle;
    protected bool _isInMinAggroRng;

    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_IdleState stateData) 
        : base(entity, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        // when entering idle stop moving
        _entity.SetVelocity(0f);
        _endIdle = false;
        SetIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        // if flip after idle is true flip enemy
        if (_flipAfterIdle)
        {
            _entity.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // if idle time is over set _endIdle true
        if (Time.time >= _startTime + _idleTime)
        {
            _endIdle = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void MakeChecks()
    {
        base.MakeChecks();

        // keep checking for player aggro
        _isInMinAggroRng = _entity.CheckMinAgro();
    }

    public void SetFlipAfterIdle(bool flip)
    {
        _flipAfterIdle = flip;
    }

    // set idle time to a random number
    private void SetIdleTime()
    {
        _idleTime = Random.Range(_stateData.minIdle, _stateData.maxIdle);
    }
}
