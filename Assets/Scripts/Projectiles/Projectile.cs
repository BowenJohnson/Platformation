using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private AttackDetails _attackDetails;
    private float _speed;

    // how far has traveled
    private float _travDist;

    private float _xStartPosition;

    [SerializeField] private float _gravity;
    [SerializeField] private float _dmgRadius;

    // time before object is destroyed after hitting ground
    [SerializeField] private float _destroyTime;

    // flag to track if gravity is currently on
    private bool _isGravOn;

    // flag to track if projectile has hit a platform
    private bool _hitGround;

    // destruction flag
    private bool _hitSomething;

    private Rigidbody2D _rb;

    private SpriteRenderer _spriteRend;

    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private Transform _dmgPos;

    // projectile SFX reference
    private ProjectileSFX _sfx;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _spriteRend = GetComponent<SpriteRenderer>();

        _sfx = GetComponent<ProjectileSFX>();

        _hitSomething = false;

        _isGravOn = false;

        // initially make uneffected by gravity and set velocity
        _rb.gravityScale = 0.0f;
        _rb.velocity = -transform.right * _speed;

        // set starting location
        _xStartPosition = transform.position.x;

        // start playing SFX
        _sfx.PlayShoot();
    }

    private void Update()
    {
        // if it hasn't hit the ground and gravity is on going on it
        // then make it angle downward
        if (!_hitGround)
        {
            _attackDetails.position = transform.position;

            if (_isGravOn)
            {
                // multiply by Mathf.Rad2Deg to get back to degrees
                // added 180 degrees because image is facing left to start
                float angle = Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg + 180;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_hitGround && !_hitSomething)
        {
            Collider2D dmgHit = Physics2D.OverlapCircle(_dmgPos.position, _dmgRadius, _whatIsPlayer);
            Collider2D groundHit = Physics2D.OverlapCircle(_dmgPos.position, _dmgRadius, _whatIsGround);

            // if hit a player then call the damage function on the player sending attack details
            // then destroy the arrow
            if (dmgHit)
            {
                // change to has hit something
                _hitSomething = true;

                // play hit entity sound
                _sfx.PlayHitEntity();

                dmgHit.transform.SendMessage("Damage", _attackDetails);               

                _spriteRend.enabled = false;
                DelayedDestroy(_destroyTime);
            }

            // if hit the ground then set flag and remove gravity and set velocity to 0
            // so it stops moving
            if (groundHit)
            {
                // change to has hit something
                _hitSomething = true;

                // play hit entity sound
                _sfx.PlayHitWall();

                _hitGround = true;
                _rb.gravityScale = 0.0f;
                _rb.velocity = Vector2.zero;

                // once it hits the ground start coroutine to start
                // a timer then destroy object when timer is up
                StartCoroutine(DelayedDestroy(_destroyTime));
            }

            // if max range has been reached and gravity isn't on turn on gravity
            if (Mathf.Abs(_xStartPosition - transform.position.x) >= _travDist && !_isGravOn)
            {
                _isGravOn = true;
                _rb.gravityScale = _gravity;
            }
        }
    }

    // function that is called when projectile is instantiated
    public void FireProjectile(float speed, float travDist, float dmg)
    {
        _speed = speed;
        _travDist = travDist;
        _attackDetails.damage = dmg;
    }

    IEnumerator DelayedDestroy(float _destroyTime)
    {
        yield return new WaitForSeconds(_destroyTime);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_dmgPos.position, _dmgRadius);
    }
}
