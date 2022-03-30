using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : State
{
    protected Data_ChargeState _stateData;

    protected bool _isInMinAggroRng;
    protected bool _isLedge;
    protected bool _isWall;
    protected bool _isChargeTimeEnd;
    protected bool _doCloseRngAction;

    public ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_ChargeState stateData) 
        : base(entity, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        // on entering this state set velocity
        // to enemy charge speed from Data_chargeState var
        _entity.SetVelocity(_stateData.chargeSpeed);

        // still charging to start
        _isChargeTimeEnd = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // if charging has gone on longer than the preset time
        // then set charge time end bool to true
        if (Time.time >= _startTime + _stateData.chargeTime)
        {
            _isChargeTimeEnd = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void MakeChecks()
    {
        base.MakeChecks();

        _isInMinAggroRng = _entity.CheckMinAgro();

        // keep checking terrain so it doesn't
        // run into a wall or off a platform
        _isLedge = _entity.CheckLedge();
        _isWall = _entity.CheckWall();

        // check if player is in range for the
        // close range ability
        _doCloseRngAction = _entity.CheckCloseRangeAction();
    }
}
