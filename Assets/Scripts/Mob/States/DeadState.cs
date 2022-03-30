using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeadState : State
{
    protected Data_DeadState _stateData;

    public DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_DeadState stateData) 
        : base(entity, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        // if there is a death particle to be used then 
        // instantiate it on enter
        if (_stateData._deathParticle)
        {
            GameObject.Instantiate(_stateData._deathParticle, _entity.aliveGameObj.transform.position, _stateData._deathParticle.transform.rotation);
        }
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
