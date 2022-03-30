using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBoss_MoveState : MoveState
{
    protected DemonBoss _demonBoss;

    protected bool _isInMaxAggroRng;

    public DemonBoss_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_MoveState stateData, DemonBoss demonBoss) 
        : base(entity, stateMachine, animBoolName, stateData)
    {
        _demonBoss = demonBoss;
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

        // if player is in min aggro range switch to player detected state
        if (_isInMinAggroRng)
        {
            _stateMachine.ChangeState(_demonBoss.playerDetectedState);
        }
        // if player isn't in long range
        else if (!_isInMaxAggroRng)
        {
            // flip and pursue again
            _demonBoss.idleState.SetFlipAfterIdle(true);

            // do a brief idle 
            _stateMachine.ChangeState(_demonBoss.idleState);
        }
        // if hit a ledge turn around
        else if (!_isLedge)
        {
            // stop, turn around, and start moving again
            _entity.SetVelocity(0f);
            _entity.Flip();
            _stateMachine.ChangeState(_demonBoss.moveState);
        }
    }

    public override void MakeChecks()
    {
        base.MakeChecks();
        _isInMaxAggroRng = _entity.CheckMaxAgro();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
