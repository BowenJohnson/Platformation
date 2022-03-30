using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    // place to store the attack position be it melee or ranged
    protected Transform _attkPos;

    protected bool _isAnimDone;
    protected bool _isInMinAggroRng;

    public AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attkPos) 
        : base(entity, stateMachine, animBoolName)
    {
        _attkPos = attkPos;
    }

    public override void Enter()
    {
        base.Enter();

        _entity.atsm.attackState = this;
        _isAnimDone = false;
        _entity.SetVelocity(0f);
    }

    //public override void Exit()
    //{
    //    base.Exit();
    //}

    //public override void LogicUpdate()
    //{
    //    base.LogicUpdate();
    //}

    public override void MakeChecks()
    {
        base.MakeChecks();

        // see if player is in min aggro range so it can transition
        // states if needed
        _isInMinAggroRng = _entity.CheckMinAgro();
    }

    //public override void PhysicsUpdate()
    //{
    //    base.PhysicsUpdate();
    //}

    // a function that will begin the attack animation
    public virtual void StartAttack()
    {

    }

    // called at correct point in the mob
    // animation to cause damage/effect
    public virtual void FinishAttack()
    {
        _isAnimDone = true;
    }
}
