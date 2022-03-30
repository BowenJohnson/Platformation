using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected Data_MeleeAttack _stateData;
    protected AttackDetails _attackDetails;

    public MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attkPos, Data_MeleeAttack stateData) 
        : base(entity, stateMachine, animBoolName, attkPos)
    {
        _stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        _attackDetails.damage = _stateData.attackDmg;
        _attackDetails.position = _entity.aliveGameObj.transform.position;
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

        _entity.GetComponent<AudioSource>().PlayOneShot(_stateData._meleeSwingSFX);

        // when animation triggers attack is will create a circle from the attack position and check if the player is in that circle
        Collider2D[] detectedObj = Physics2D.OverlapCircleAll(_attkPos.position, _stateData.attackRadius, _stateData.whatIsPlayer);


        // loop through everything that gets detected
        foreach (Collider2D collider in detectedObj)
        {
            // if hit then play the hit SFX
            _entity.GetComponent<AudioSource>().PlayOneShot(_stateData._meleeHitSFX);

            // call damage function in collider
            collider.transform.SendMessage("Damage", _attackDetails);
        }
    }
}
