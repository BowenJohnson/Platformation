using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobWml_LookForPlayerState : LookForPlayerState
{
    private GobWml _gobWml;

    public GobWml_LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_LookForPlayerState stateData, GobWml gobWml) 
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

        // look around for player if player is spotted
        // switch states an get ready to act again if not
        // go back to moving around

        // if player is in min aggro range change
        // to player detected state
        if (_isInMinAggroRng)
        {
            _stateMachine.ChangeState(_gobWml.playerDetectedState);
        }
        // if turn time is over change to move state
        else if (_isTurnTimeOver)
        {
            _stateMachine.ChangeState(_gobWml.moveState);
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
