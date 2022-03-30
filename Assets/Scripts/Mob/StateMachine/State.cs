using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected FiniteStateMachine _stateMachine;
    protected Entity _entity;
    protected string _animatorBoolName;

    // public with protected set to give other states access to their timers
    // to check for completion
    public float _startTime { get; protected set; }

    public State(Entity entity, FiniteStateMachine stateMachine, string animBoolName)
    {
        _entity = entity;
        _stateMachine = stateMachine;
        _animatorBoolName = animBoolName;
    }

    // behavior and animation when mob enters state
    public virtual void Enter()
    {
        _startTime = Time.time;
        _entity.anim.SetBool(_animatorBoolName, true);
        MakeChecks();
    }

    // behavior when mob exits state
    // set current animator bool to false
    // in the animator
    public virtual void Exit()
    {
        _entity.anim.SetBool(_animatorBoolName, false);
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        MakeChecks();
    }

    // any reoccuring checks the enemy needs
    // to make will go in this override
    public virtual void MakeChecks()
    {

    }
}
