using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobWml_MeleeAttackState : MeleeAttackState
{
    private GobWml _gobWml;

    public GobWml_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attkPos, Data_MeleeAttack stateData, GobWml gobWml) 
        : base(entity, stateMachine, animBoolName, attkPos, stateData)
    {
        _gobWml = gobWml;
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

        // if attack animation is done check player range
        if (_isAnimDone)
        {
            // if player is in min aggro range then
            // change to player detected state
            if(_isInMinAggroRng)
            {
                _stateMachine.ChangeState(_gobWml.playerDetectedState);
            }
            // else look for the player
            else
            {
                _stateMachine.ChangeState(_gobWml.lookForPlayerState);
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
