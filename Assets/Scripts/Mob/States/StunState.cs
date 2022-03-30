using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// make enemy stop moving for a set amount of time
// then proceed to next state
public class StunState : State
{
    protected Data_StunState _stateData;

    protected bool _isStunOver;
    protected bool _isGrounded;
    protected bool _isMoveStop;
    protected bool _isInMinAggroRng;
    protected bool _doCloseRngAction;

    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_StunState stateData) 
        : base(entity, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        _isStunOver = false;
        _isMoveStop = false;
        _entity.SetVelocity(_stateData.knockbackSpeed, _stateData.knockbackDir, _entity.damageDir);
    }

    public override void Exit()
    {
        base.Exit();

        // reset stun resist when stun ends
        _entity.ResetStunResist();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= _startTime + _stateData.stunnedTime)
        {
            _isStunOver = true;
        }

        // do a ground check while checking for time to give a buffer
        // so the mob has time to get airborn so it doesn't return true immediately
        if (_isGrounded && Time.time >= _startTime + _stateData.knockbackTime && !_isMoveStop)
        {
            _isMoveStop = true;
            _entity.SetVelocity(0f);
        }
    }

    public override void MakeChecks()
    {
        base.MakeChecks();

        _isGrounded = _entity.CheckGround();

        // make aggro and action checks so that these actions
        // can be performed as soon as stun is over
        _doCloseRngAction = _entity.CheckCloseRangeAction();
        _isInMinAggroRng = _entity.CheckMinAgro();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
