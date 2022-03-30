using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobBow_StunState : StunState
{
    private GobBow _gobBow;

    public GobBow_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_StunState stateData, GobBow gobBow) 
        : base(entity, stateMachine, animBoolName, stateData)
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

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // if stun time is over and player is in min aggro
        // change to player detected else look for player
        if (_isStunOver)
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
}
