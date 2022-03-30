using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class controls the Demon Boss monster
public class DemonBoss : Entity
{
    // the range of hits taken before special move is activated
    private int _hitRangeUpper = 6;
    private int _hitRangeLower = 3;

    // hit counters for special attack activation
    public int _specialHitMax { get; private set; }
    public int _currHits { get; private set; }

    // bool to trigger when to act
    public bool waitOver { get; set; }

    // AI states
    public DemonBoss_WaitState waitState { get; private set; }
    public DemonBoss_IdleState idleState { get; private set; }
    public DemonBoss_MoveState moveState { get; private set; }
    public DemonBoss_PlayerDetectedState playerDetectedState {get; private set; }
    public DemonBoss_OverhandAttackState overhandAttackState { get; private set; }
    public DemonBoss_BackhandAttackState backhandAttackState { get; private set; }
    public DemonBoss_StompAttackState stompAttackState { get; private set; }
    public DemonBoss_DeadState deadState { get; private set; }


    [Header("AI State Data")]
    [SerializeField] private Data_IdleState _waitStateData;
    [SerializeField] private Data_IdleState _idleStateData;
    [SerializeField] private Data_MoveState _moveStateData;
    [SerializeField] private Data_PlayerDetected _playerDetectedData;
    [SerializeField] private Data_MeleeAttack _overhandAttackStateData;
    [SerializeField] private Data_MeleeAttack _backhandAttackStateData;
    [SerializeField] private Data_MeleeAttack _stompAttackStateData;
    [SerializeField] private Data_DeadState _deadStateData;

    // taller mob needs more detection transforms
    [Header("Extra Boss Check Transforms")]
    [SerializeField] private Transform _playerCheck2;
    [SerializeField] private Transform _playerCheck3;
    [SerializeField] private Transform _playerCheck4;
    [SerializeField] private Transform _playerCheck5;

    [Header("Attack Transforms")]
    [SerializeField] private Transform _overhandAttackPos;
    [SerializeField] private Transform _backhandAttackPos;
    [SerializeField] private Transform _stompAttackPos;

    [SerializeField] private Transform _overhandParticlePos;

    [Header("Fireball Prefab & Transforms")]
    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private Transform _stompFlametPos;
    [SerializeField] private Transform _fireballFrontPos;
    [SerializeField] private Transform _fireballBackPos;
    [SerializeField] private float _rngSpeed;
    [SerializeField] private float _rngDist;
    [SerializeField] private float _rngDmg;

    // reference to the SFX controller
    private BasicMobSFX _sfx;

    // Start is called before the first frame update
    public override void Start()
    {
        // fire up the start() from entity
        base.Start();

        // reset hit counter
        ResetSpecialHits();

        waitOver = false;

        // instantiate each of the possible states
        waitState = new DemonBoss_WaitState(this, stateMachine, "wait", _idleStateData, this);
        idleState = new DemonBoss_IdleState(this, stateMachine, "idle", _idleStateData, this);
        moveState = new DemonBoss_MoveState(this, stateMachine, "move", _moveStateData, this);
        playerDetectedState = new DemonBoss_PlayerDetectedState(this, stateMachine, "playerDetected", _playerDetectedData, this);
        overhandAttackState = new DemonBoss_OverhandAttackState(this, stateMachine, "overhandAttack", _overhandAttackPos, _overhandAttackStateData, this, _overhandParticlePos);        
        backhandAttackState = new DemonBoss_BackhandAttackState(this, stateMachine, "backhandAttack", _backhandAttackPos, _backhandAttackStateData, this);
        stompAttackState = new DemonBoss_StompAttackState(this, stateMachine, "stompAttack", _stompAttackPos, _stompAttackStateData, this, 
            _fireballPrefab, _stompFlametPos, _fireballFrontPos, _fireballBackPos, _rngSpeed, _rngDist, _rngDmg);
        deadState = new DemonBoss_DeadState(this, stateMachine, "dead", _deadStateData, this, _fireballBackPos);

        _sfx = GetComponent<BasicMobSFX>();

        //stateMachine.Initialize(moveState);
        stateMachine.Initialize(waitState);
    }

    // override damage to track current hits
    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        // if dead then change to dead state
        if (_isDead)
        {
            // add one to the kill counter
            FindObjectOfType<HeroController>().AddMobsKilled();

            stateMachine.ChangeState(deadState);
        }
        else
        {
            _currHits++;
        }       
    }

    // resets current hit counter and special hit max counter
    // using a range for special hit activation so the attack will be
    // less predictable by the player
    public void ResetSpecialHits()
    {
        _currHits = 0;
        _specialHitMax = Random.Range(_hitRangeLower, _hitRangeUpper);
    }

    // override the func to adapt to having an extra transform
    // if either raycast hits return true else return false
    public override bool CheckMinAgro()
    {
        if (base.CheckMinAgro())
        {
            return true;
        }
        else if (Physics2D.Raycast(_playerCheck2.position, -aliveGameObj.transform.right, entityData.minAggroDist, entityData.whatIsPlayer))
        {
            return true;
        }
        else if (Physics2D.Raycast(_playerCheck3.position, -aliveGameObj.transform.right, entityData.minAggroDist, entityData.whatIsPlayer))
        {
            return true;
        }
        else if (Physics2D.Raycast(_playerCheck4.position, -aliveGameObj.transform.right, entityData.minAggroDist, entityData.whatIsPlayer))
        {
            return true;
        }
        else if (Physics2D.Raycast(_playerCheck5.position, -aliveGameObj.transform.right, entityData.minAggroDist, entityData.whatIsPlayer))
        {
            return true;
        }
        else
        {
            return false;
        }      
    }

    // checks if player is in max aggro range using all 4 transforms
    // if not that means the player is behind the boss
    public override bool CheckMaxAgro()
    {
        if (base.CheckMaxAgro())
        {
            return true;
        }
        else if (Physics2D.Raycast(_playerCheck2.position, -aliveGameObj.transform.right, entityData.maxAggroDist, entityData.whatIsPlayer))
        {
            return true;
        }
        else if (Physics2D.Raycast(_playerCheck3.position, -aliveGameObj.transform.right, entityData.maxAggroDist, entityData.whatIsPlayer))
        {
            return true;
        }
        else if (Physics2D.Raycast(_playerCheck4.position, -aliveGameObj.transform.right, entityData.maxAggroDist, entityData.whatIsPlayer))
        {
            return true;
        }
        else if (Physics2D.Raycast(_playerCheck5.position, -aliveGameObj.transform.right, entityData.maxAggroDist, entityData.whatIsPlayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // draw the extra needed detection transforms in unity
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // circles for attack spheres
        Gizmos.DrawWireSphere(_overhandAttackPos.position, _overhandAttackStateData.attackRadius);
        Gizmos.DrawWireSphere(_backhandAttackPos.position, _backhandAttackStateData.attackRadius);
        Gizmos.DrawWireSphere(_stompAttackPos.position, _stompAttackStateData.attackRadius);

        // multiply the vector2 by negative facing direction (because sprite starts facing left) so that the gizmos
        // will flip in the scene view when the mob flips direction
        Gizmos.DrawWireSphere(_playerCheck2.position + (Vector3)(Vector2.left * -facingDir * entityData.closeRngActDist), 0.2f);
        Gizmos.DrawWireSphere(_playerCheck2.position + (Vector3)(Vector2.left * -facingDir * entityData.minAggroDist), 0.2f);
        Gizmos.DrawWireSphere(_playerCheck2.position + (Vector3)(Vector2.left * -facingDir * entityData.maxAggroDist), 0.2f);

        Gizmos.DrawWireSphere(_playerCheck3.position + (Vector3)(Vector2.left * -facingDir * entityData.closeRngActDist), 0.2f);
        Gizmos.DrawWireSphere(_playerCheck3.position + (Vector3)(Vector2.left * -facingDir * entityData.minAggroDist), 0.2f);
        Gizmos.DrawWireSphere(_playerCheck3.position + (Vector3)(Vector2.left * -facingDir * entityData.maxAggroDist), 0.2f);

        Gizmos.DrawWireSphere(_playerCheck4.position + (Vector3)(Vector2.left * -facingDir * entityData.closeRngActDist), 0.2f);
        Gizmos.DrawWireSphere(_playerCheck4.position + (Vector3)(Vector2.left * -facingDir * entityData.minAggroDist), 0.2f);
        Gizmos.DrawWireSphere(_playerCheck4.position + (Vector3)(Vector2.left * -facingDir * entityData.maxAggroDist), 0.2f);

        Gizmos.DrawWireSphere(_playerCheck5.position + (Vector3)(Vector2.left * -facingDir * entityData.closeRngActDist), 0.2f);
        Gizmos.DrawWireSphere(_playerCheck5.position + (Vector3)(Vector2.left * -facingDir * entityData.minAggroDist), 0.2f);
        Gizmos.DrawWireSphere(_playerCheck5.position + (Vector3)(Vector2.left * -facingDir * entityData.maxAggroDist), 0.2f);
    }
}
