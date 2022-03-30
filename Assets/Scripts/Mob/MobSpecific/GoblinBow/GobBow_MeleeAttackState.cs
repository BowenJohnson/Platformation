using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobBow_MeleeAttackState : MeleeAttackState
{
    private GobBow _gobBow;

    public GobBow_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attkPos, Data_MeleeAttack stateData, GobBow gobBow) 
        : base(entity, stateMachine, animBoolName, attkPos, stateData)
    {
        _gobBow = gobBow;
    }

    public override void Enter()
    {
        base.Enter();
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

        // if the animation is finished
        if (_isAnimDone)
        {
            // if player is in min aggro range
            if (_isInMinAggroRng)
            {
                // change state to player detected
                _stateMachine.ChangeState(_gobBow.playerDetectedState);
            }
            else if (!_isInMinAggroRng)
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
