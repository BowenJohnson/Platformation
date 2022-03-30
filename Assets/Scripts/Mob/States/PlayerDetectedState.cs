using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State
{
    protected Data_PlayerDetected _stateData;

    // bools for checking if player is
    // in min and max aggro ranges
    protected bool _isInMinAggroRng;
    protected bool _isInMaxAggroRng;

    // mobs will have a long range and
    // short range actions depending on character
    // i.e. shoot arrow vs. melee
    protected bool _doLongRngAction;
    protected bool _doCloseRngAction;

    // check for ledges while looking for player
    protected bool _isLedge;

    public PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_PlayerDetected stateData) 
        : base(entity, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        _entity.SetVelocity(0);

        _doLongRngAction = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= _startTime + _stateData.longRangeActionTime)
        {
            _doLongRngAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


    public override void MakeChecks()
    {
        base.MakeChecks();

        // check if the player is in the min and max aggro
        // ranges and set those bools accordingly
        _isInMinAggroRng = _entity.CheckMinAgro();
        _isInMaxAggroRng = _entity.CheckMaxAgro();

        // check if player is in range for the close range ability
        _doCloseRngAction = _entity.CheckCloseRangeAction();

        // check for ledges also
        _isLedge = _entity.CheckLedge();
    }
}
