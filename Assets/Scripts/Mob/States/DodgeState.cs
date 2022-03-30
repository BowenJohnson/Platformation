using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : State
{
    protected Data_DodgeState _stateData;

    // bools to check if mob can do close action
    // if player is in max aggro range
    // if the dodge cool down is done
    // if mob is on the ground
    protected bool _doCloseRngAct;
    protected bool _isInMaxAggroRng;
    protected bool _isDodgeDone;
    protected bool _isGrounded;

    public DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_DodgeState stateData) 
        : base(entity, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        // set dodge done flag to false when entering this state
        _isDodgeDone = false;

        // set the velocity of the dodge using the data speed, angle,
        // and use negative facing dir to jump away from hero
        _entity.SetVelocity(_stateData.dodgeSpeed, _stateData.dodgeAngle, -_entity.facingDir);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // check if enemy has already dodged and check if on ground
        // if so then set dodgeDone to true
        if(Time.time >= _startTime + _stateData.dodgeTimer && _isGrounded)
        {
            _isDodgeDone = true;
        }
    }

    public override void MakeChecks()
    {
        base.MakeChecks();

        // set those bools from entity class actions
        _doCloseRngAct = _entity.CheckCloseRangeAction();
        _isInMaxAggroRng = _entity.CheckMaxAgro();
        _isGrounded = _entity.CheckGround();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
