using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    // class that is holding all the variable data
    protected Data_MoveState _stateData;

    // bools for detecting wall, ledge,
    // and player in aggro range
    protected bool _isWall;
    protected bool _isLedge;
    protected bool _isInMinAggroRng;

    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_MoveState stateData) 
        : base(entity, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        _entity.SetVelocity(_stateData.movementSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();     
    }

    public override void MakeChecks()
    {
        base.MakeChecks();

        // call entitiy functions to set bools
        // for wall, ledge, and player aggro range
        _isWall = _entity.CheckWall();
        _isLedge = _entity.CheckLedge();
        _isInMinAggroRng = _entity.CheckMinAgro();
    }
}
