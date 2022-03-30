using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobWml_StunState : StunState
{
    private GobWml _gobWml;
    public GobWml_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_StunState stateData, GobWml gobWml) 
        : base(entity, stateMachine, animBoolName, stateData)
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

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // check if stun is over
        if (_isStunOver)
        {
            // if can do close range action switch to melee attack state and punch face
            if (_doCloseRngAction)
            {
                _stateMachine.ChangeState(_gobWml.meleeAttackState);
            }
            // else if player is in min aggro range then CHARGE!
            else if(_isInMinAggroRng)
            {
                _stateMachine.ChangeState(_gobWml.chargeState);
            }
            // otherwise flip immediately and start looking for player
            else
            {
                _gobWml.lookForPlayerState.SetFlipNow(true);
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
}
