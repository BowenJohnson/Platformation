using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobBow_DodgeState : DodgeState
{
    private GobBow _gobBow;

    public GobBow_DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_DodgeState stateData, GobBow gobBow) 
        : base(entity, stateMachine, animBoolName, stateData)
    {
        _gobBow = gobBow;
    }

    public override void Enter()
    {
        // play dodge SFX
        _gobBow.PlaySFX(_stateData._dodgeSFX);

        base.Enter();
    }

    //public override void Exit()
    //{
    //    base.Exit();
    //}

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isDodgeDone)
        {
            // if player is in max aggro range and mob
            // can do close range action the change to melee attack state
            if (_isInMaxAggroRng && _doCloseRngAct)
            {
                _stateMachine.ChangeState(_gobBow.meleeAttackState);
            }
            // if player is detected but not close enough for dodge/melee
            // switch to ranged attack state
            else if (_isInMaxAggroRng && !_doCloseRngAct)
            {
                _stateMachine.ChangeState(_gobBow.meleeAttackState);
            }
            // if player isn't in max aggro range then look for player
            else if (!_isInMaxAggroRng)
            {
                _stateMachine.ChangeState(_gobBow.lookForPlayerState);
            }
        }
    }

    //public override void MakeChecks()
    //{
    //    base.MakeChecks();
    //}

    //public override void PhysicsUpdate()
    //{
    //    base.PhysicsUpdate();
    //}
}
