using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayerState : State
{
    protected Data_LookForPlayerState _stateData;

    protected bool _isInMinAggroRng;

    // keeping track if it has made all
    // of its turns and spent all its time
    // looking in those directions
    protected bool _isDoneTurning;
    protected bool _isTurnTimeOver;

    // flag to flip around immediately looking for player
    protected bool _turnNow;

    // keep track of time did last turn
    protected float _lastTurnTime;

    // keep track of number of turns made
    protected int _numTurnsDone;

    public LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_LookForPlayerState stateData) 
        : base(entity, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        // initialize all bools as false and timers to 0;
        _isDoneTurning = false;
        _isTurnTimeOver = false;

        _lastTurnTime = _startTime;
        _numTurnsDone = 0;

        // stop moving just in case
        _entity.SetVelocity(0);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // if turn now is true flip around
        // and start turn timer, increment turns
        // and return turn now to false to stop
        if (_turnNow)
        {
            _entity.Flip();
            _lastTurnTime = Time.time;
            _numTurnsDone++;
            _turnNow = false;
        }
        // if current time is >= last time flipped + time between flips
        // and havent finished all the set number of turns
        else if (Time.time >= _lastTurnTime + _stateData.turnTime && !_isDoneTurning)
        {
            _entity.Flip();
            _lastTurnTime = Time.time;
            _numTurnsDone++;
        }

        // if all flips have been made set _isDoneTurning true
        if (_numTurnsDone >= _stateData.numTurns)
        {
            _isDoneTurning = true;
        }

        // done final turn then wait for one more turn time and reset the turn time over flag
        if (Time.time >= _lastTurnTime + _stateData.turnTime && _isDoneTurning)
        {
            _isTurnTimeOver = true;
        }
    }

    public override void MakeChecks()
    {
        base.MakeChecks();

        // set bool to true if player is spotted again
        _isInMinAggroRng = _entity.CheckMinAgro();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetFlipNow(bool flip)
    {
        _turnNow = flip;
    }
}
