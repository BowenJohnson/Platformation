using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobWml_DeadState : DeadState
{
    private GobWml _gobWml;

    public GobWml_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_DeadState stateData, GobWml gobWml) 
        : base(entity, stateMachine, animBoolName, stateData)
    {
        _gobWml = gobWml;
    }

    public override void Enter()
    {
        base.Enter();

        // stop just in case
        _entity.SetVelocity(0);

        // set layer to dead so it won't collide with anything else
        _entity.aliveGameObj.layer = LayerMask.NameToLayer("Dead");

        // play the death SFX
        _gobWml.PlaySFX(_stateData._deadSFX);

        // delayed destroy so the SFX have time to play
        _gobWml.DelayDestroy(_stateData._destroyDelayTime);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
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
