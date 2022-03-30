using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// all enemies will inherit this
public class Entity : MonoBehaviour
{
    [Header("Entity Variables")]
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _currentStunResist;

    [Header("Check Transforms")]
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _ledgeCheck;
    [SerializeField] private Transform _playerCheck;
    [SerializeField] private Transform _groundCheck;

    [Header("Hit Particle & Transform")]
    [SerializeField] private GameObject _hitParticle;
    [SerializeField] private Transform _hitLoc;

    // timer for tracking stun tracker reset
    private float _lastDmgTime;

    // temp var that will be used to update the actual velocity
    private Vector2 velocityTemp;

    // tracking if mob is stunned
    protected bool _isStunned;

    // tracking if mob is dead
    protected bool _isDead;

    // state machine for AI and data for entity object
    public FiniteStateMachine stateMachine;

    // store entity specific data like hp, aggro ranges, and...etc
    [Header("Entity Data")]
    public Data_Entity entityData;

    // keeps track of which side attack came from for knockback
    public int damageDir { get; private set; }

    // keeps track of which direction the mob is facing
    public int facingDir { get; private set; }

    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    // reference to the alive child object in unity
    public GameObject aliveGameObj { get; private set; }

    // this is the go between for the animator and the state machine
    public AnimToStaMac atsm { get; private set; }

    public virtual void Start()
    {
        // get references to all the components of the current entity
        aliveGameObj = transform.Find("Alive").gameObject;
        rb = aliveGameObj.GetComponent<Rigidbody2D>();
        anim = aliveGameObj.GetComponent<Animator>();
        atsm = aliveGameObj.GetComponent<AnimToStaMac>();
        stateMachine = new FiniteStateMachine();

        _currentHealth = entityData.maxHealth;
        _currentStunResist = entityData.stunResistPoints;
        facingDir = -1;
    }

    // each time update is called call update on the state
    public virtual void Update()
    {
        stateMachine.currState.LogicUpdate();

        anim.SetFloat("yVel", rb.velocity.y);

        // if current time is greater than last damage time plus
        // stun recovery time then reset stun variables via reset function
        if (Time.time >= _lastDmgTime + entityData.stunRecoverTime)
        {
            ResetStunResist();
        }
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currState.PhysicsUpdate();
    }

    // do any needed calculations to the temp velocity
    // then set the temp velocity to the actual
    public virtual void SetVelocity(float velocity)
    {
        velocityTemp.Set(facingDir * velocity, rb.velocity.y);
        rb.velocity = velocityTemp;
    }

    // use this overload to take in more parameters for other uses like
    // making the mob get knocked back or dodge
    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityTemp.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = velocityTemp;
    }

    // *** can override this in mob specific class i.e. SBW ***
    // *** by changing _wallCheck to protected              ***
    // cast ray right to check for walls ahead
    public virtual bool CheckWall()
    {
        // set for a left facing sprite will need to change for right facing
        // otherwise the ray hits its own collider and it just flips in place
        // layer mask arg so that monsters won't think each other are walls and pass through each other
        return Physics2D.Raycast(_wallCheck.position, -aliveGameObj.transform.right, entityData.wallCheckDist, ~(LayerMask.GetMask("Enemy", "Player", "Wanderer", "Aura", "Dead")));
    }

    // cast ray down to check if there is ground ahead
    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(_ledgeCheck.position, Vector2.down, entityData.ledgeCheckDist, entityData.whatIsGround);
    }

    // track when mob has hit ground after being knocked in the air and etc.
    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, entityData.groundCheckRadi, entityData.whatIsGround);
    }

    // change the facing direction int and flip sprite
    // 180 so rays will continue to point the right direction
    public virtual void Flip()
    {
        facingDir *= -1;
        aliveGameObj.transform.Rotate(0f, 180f, 0f);
    }

    // draws rays to detect the player
    // min aggro starts aggresive chase
    public virtual bool CheckMinAgro()
    {
        // ****** make transform negative right sprites are facing left to start
        return Physics2D.Raycast(_playerCheck.position, -aliveGameObj.transform.right, entityData.minAggroDist, entityData.whatIsPlayer);
    }
    
    // max aggro is how far player needs
    // to get before aggro ends
    public virtual bool CheckMaxAgro()
    {
        // ****** make transform negative right sprites are facing left to start
        return Physics2D.Raycast(_playerCheck.position, -aliveGameObj.transform.right, entityData.maxAggroDist, entityData.whatIsPlayer);
    }

    // draws ray to detect player
    // check for close range mob ability
    public virtual bool CheckCloseRangeAction()
    {
        // ****** make transform negative right sprites are facing left to start
        return Physics2D.Raycast(_playerCheck.position, -aliveGameObj.transform.right, entityData.closeRngActDist, entityData.whatIsPlayer);
    }

    // reset the stun bool and stun resist points
    public virtual void ResetStunResist()
    {
        _isStunned = false;
        _currentStunResist = entityData.stunResistPoints;
    }

    // takes in attack details type and uses it to determine
    // attack position for knockback and damage amount from
    // the attack to subtract from hp
    public virtual void Damage(AttackDetails attackDetails)
    {
        // reset time from last damage for stun tracker reset
        _lastDmgTime = Time.time;

        // subtract damage from current hp
        _currentHealth -= attackDetails.damage;

        // subtract damage from stun damage amount
        _currentStunResist -= attackDetails.stunDmg;

        // knock mob up damageBounce to signify a hit
        DamageBounce(entityData.damageBounce);

        // instantiate hit particles here later
        Instantiate(_hitParticle, _hitLoc.position, _hitParticle.transform.rotation);

        // if greater x position hit came from the right
        if (attackDetails.position.x > aliveGameObj.transform.position.x)
        {
            damageDir = -1;
        }
        // less x position hit came fromt the left
        else
        {
            damageDir = 1;
        }

        if (_currentStunResist <= 0)
        {
            _isStunned = true;
        }

        if (_currentHealth <= 0)
        {
            _isDead = true;
        }
    }

    // function that will bounce enemy back a little when hit
    public virtual void DamageBounce(float velocity)
    {
        velocityTemp.Set(rb.velocity.x, velocity);
        rb.velocity = velocityTemp;
    }

    public void DelayDestroy(float time)
    {
        StartCoroutine(DelayedDestroy(time));
    }

    private IEnumerator DelayedDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    // draw all the detection circles in unity scene
    public virtual void OnDrawGizmos()
    {
        // line draw is set for left facing sprite will need to override it for right
        Gizmos.DrawLine(_wallCheck.position, _wallCheck.position + (Vector3)(Vector2.left * -facingDir * entityData.wallCheckDist));
        Gizmos.DrawLine(_ledgeCheck.position, _ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDist));

        // multiply the vector2 by negative facing direction (because sprite starts facing left) so that the gizmos
        // will flip in the scene view when the mob flips direction
        Gizmos.DrawWireSphere(_playerCheck.position + (Vector3)(Vector2.left * -facingDir * entityData.closeRngActDist), 0.2f);
        Gizmos.DrawWireSphere(_playerCheck.position + (Vector3)(Vector2.left * -facingDir * entityData.minAggroDist), 0.2f);
        Gizmos.DrawWireSphere(_playerCheck.position + (Vector3)(Vector2.left * -facingDir * entityData.maxAggroDist), 0.2f);
    }
}
