using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobBow_DeadState : DeadState
{
    private GobBow _gobBow;

    public GobBow_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_DeadState stateData, GobBow gobBow) 
        : base(entity, stateMachine, animBoolName, stateData)
    {
        _gobBow = gobBow;
    }

    public override void Enter()
    {
        base.Enter();

        // stop just in case
        _entity.SetVelocity(0);

        // set layer to dead so it won't collide with anything else
        _entity.aliveGameObj.layer = LayerMask.NameToLayer("Dead");

        // play the death SFX
        _gobBow.PlaySFX(_stateData._deadSFX);

        // delayed destroy so the SFX have time to play
        _gobBow.DelayDestroy(_stateData._destroyDelayTime);
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
