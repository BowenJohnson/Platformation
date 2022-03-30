using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBW_DeadState : DeadState
{
    private SBW _sbw;

    public SBW_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_DeadState stateData, SBW sbw) 
        : base(entity, stateMachine, animBoolName, stateData)
    {
        _sbw = sbw;
    }

    public override void Enter()
    {
        base.Enter();

        // stop just in case
        _entity.SetVelocity(0);

        // this will turn off collisions after mob is dead
        _entity.aliveGameObj.GetComponent<Rigidbody2D>().simulated = false;

        // turn off the sprite renderer
        _entity.aliveGameObj.GetComponent<SpriteRenderer>().enabled = false;

        // play the death SFX
        _sbw.PlaySFX(_stateData._deadSFX);

        // delayed destroy so the SFX have time to play
        _sbw.DelayDestroy(_stateData._destroyDelayTime);
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

    public override string ToString()
    {
        return base.ToString();
    }
}
