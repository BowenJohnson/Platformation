using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : AttackState
{
    protected Data_RangedAttackState _stateData;

    protected GameObject _projectile;
    protected Projectile _projScript;

    public RangedAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attkPos, Data_RangedAttackState stateData) 
        : base(entity, stateMachine, animBoolName, attkPos)
    {
        _stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
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

    public override void StartAttack()
    {
        base.StartAttack();

        // instantiate projectile object on attack position using its state data 
        _projectile =  GameObject.Instantiate(_stateData.projectile, _attkPos.position, _attkPos.rotation);

        // get access to projectile script to call fire function
        _projScript = _projectile.GetComponent<Projectile>();

        // call fire function from script
        _projScript.FireProjectile(_stateData.projSpeed, _stateData.projTravDist, _stateData.projDmg);
    }
}
