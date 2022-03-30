using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class controls what attack the demon boss does when a player is in or leaves attack range
public class DemonBoss_PlayerDetectedState : PlayerDetectedState
{
    private DemonBoss _demonBoss;

    public DemonBoss_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_PlayerDetected stateData, DemonBoss demonBoss) 
        : base(entity, stateMachine, animBoolName, stateData)
    {
        _demonBoss = demonBoss;
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

        // if can do close range action 
        if (_doCloseRngAction)
        {
            // attack type percentiles:
            // 10% special attack
            // 35% overhand attack
            // 35% backhand attack

            // get random number to determine which attack
            // (using magic number range for percentile based attack)
            int randAttkNum = Random.Range(1, 100);

            // Decide which attack to do
            // if hits taken == hit count max do special
            // or a 10% chance to do special attack
            if (_demonBoss._currHits >= _demonBoss._specialHitMax || randAttkNum > 89)
            {
                // do stomp attack
                _stateMachine.ChangeState(_demonBoss.stompAttackState);
            }
            else
            {
                //  if randAttkNum > 45  do overhand attack
                // 45% chance to do either attack
                if (randAttkNum > 45)
                {
                    _stateMachine.ChangeState(_demonBoss.overhandAttackState);
                }
                //  else do backhand attack
                else
                {
                    _stateMachine.ChangeState(_demonBoss.backhandAttackState);
                }
            }
        }
        // if player gets out of max aggro range start looking for player
        else if (!_isInMaxAggroRng)
        {
            // flip and pursue again
            _demonBoss.idleState.SetFlipAfterIdle(true);

            // do a brief idle 
            _stateMachine.ChangeState(_demonBoss.idleState);
        }
        else if (_isInMaxAggroRng)
        {
            _stateMachine.ChangeState(_demonBoss.moveState);
        }
        // if mob comes to an edge while player is detected then turn around and walk away
        else if (!_isLedge)
        {
            _entity.Flip();
            _stateMachine.ChangeState(_demonBoss.moveState);
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
