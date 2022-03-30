using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBoss_DeadState : DeadState
{
    private DemonBoss _demonBoss;
    private Transform _deathBurstLoc;

    // reference to level controller for when he dies
    private BossLevelController _levelController;

    public DemonBoss_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_DeadState stateData, 
        DemonBoss demonBoss, Transform deathBurstLoc) 
        : base(entity, stateMachine, animBoolName, stateData)
    {
        _demonBoss = demonBoss;
        _deathBurstLoc = deathBurstLoc;
        _levelController = Object.FindObjectOfType<BossLevelController>();
    }

    public override void Enter()
    {
        base.Enter();

        // stop just in case
        _entity.SetVelocity(0);

        _levelController.BossDied();

        // set layer to dead so it won't collide with anything else
        _entity.aliveGameObj.layer = LayerMask.NameToLayer("Dead");

        // play the death SFX
        _entity.GetComponent<AudioSource>().PlayOneShot(_stateData._deadSFX);

        // instantiate death particle on position using its state data 
        Object.Instantiate(_stateData._deathParticle, _deathBurstLoc.position, _stateData._deathParticle.transform.rotation);

        // delayed destroy so the SFX have time to play
        _demonBoss.DelayDestroy(_stateData._destroyDelayTime);
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
