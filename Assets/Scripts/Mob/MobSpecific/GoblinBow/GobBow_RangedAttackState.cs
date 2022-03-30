using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobBow_RangedAttackState : RangedAttackState
{
    private GobBow _gobBow;

    public GobBow_RangedAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attkPos, 
        Data_RangedAttackState stateData, GobBow gobBow) 
        : base(entity, stateMachine, animBoolName, attkPos, stateData)
    {
        _gobBow = gobBow;
    }

    public override void Enter()
    {
        base.Enter();

        _gobBow.PlaySFX(_stateData._rangedAttackSFX);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // if the animation is finished and player is in min aggro range
        // change to player detected state else look for player state
        if (_isAnimDone)
        {
            if (_isInMinAggroRng)
            {
                _stateMachine.ChangeState(_gobBow.playerDetectedState);
            }
            else
            {
                _stateMachine.ChangeState(_gobBow.lookForPlayerState);
            }
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

    public override void StartAttack()
    {
        base.StartAttack();
    }
}
