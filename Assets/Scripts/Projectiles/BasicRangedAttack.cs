using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class controls the basic ranged attack projectile
public class BasicRangedAttack : MagicAttack
{
    [Header("Basic Ranged Attack Transforms")]
    [SerializeField] private GameObject _hitParticle;
    [SerializeField] private GameObject _fizzleParticle;
    private ProjectileSFX _sfx;
    private bool _hitSomething;
    private bool _fizzle;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRend = GetComponent<SpriteRenderer>();
        _sfx = GetComponent<ProjectileSFX>();

        // has not hit anything or fizzled yet
        _hitSomething = false;
        _fizzle = false;

        // initially make uneffected by gravity and set velocity
        _rb.gravityScale = 0.0f;
        _rb.velocity = transform.right * _speed;

        // set starting location
        _xStartPosition = transform.position.x;

        // start shooting sfx
        _sfx.PlayShoot();
    }

    private void Update()
    {
        _attackDetails.position = transform.position;
    }

    private void FixedUpdate()
    {
        CheckHit();
    }

    private void CheckHit()
    {
        Collider2D dmgHit = Physics2D.OverlapCircle(_dmgPos.position, _dmgRadius, _whatIsEnemy);
        Collider2D groundHit = Physics2D.OverlapCircle(_dmgPos.position, _dmgRadius, _whatIsGround);

        // if hit a player then call the damage function on the player sending attack details
        // then destroy the arrow
        if (dmgHit && !_fizzle && !_hitSomething)
        {
            // change to has hit something
            _hitSomething = true;

            // instantiate hit partical
            Instantiate(_hitParticle, transform.position, transform.rotation);

            dmgHit.transform.parent.SendMessage("Damage", _attackDetails);

            _sfx.PlayHitEntity();
            _spriteRend.enabled = false;
            DelayedDestroy(_destroyTime);
        }

        // if hit the ground then set flag and set velocity to 0 so it stops moving
        if (groundHit && !_fizzle && !_hitSomething)
        {
            // change to has hit something
            _hitSomething = true;

            _hitGround = true;
            _rb.velocity = Vector2.zero;

            // instantiate hit partical
            Instantiate(_hitParticle, transform.position, transform.rotation);

            // play hit sound turn off sprite and set delayed destroy
            _sfx.PlayHitEntity();
            _spriteRend.enabled = false;
            DelayedDestroy(_destroyTime);
        }

        // if max range has been reached instantiate fizzle partical and destroy object
        if (Mathf.Abs(_xStartPosition - transform.position.x) >= _travDist && !_hitSomething)
        {
            if (_fizzle == false)
            {
                // turn on the fizzle flag to stop collisions
                _fizzle = true;

                // instantiate fizzle partical
                Instantiate(_fizzleParticle, transform.position, transform.rotation, transform);

                // give the particles a head start
                StartCoroutine(FizzleDelay(_fizzleDelayTime));

                // do a time delayed fizzle out destroy
                StartCoroutine(FizzleOut(_destroyTime));
            }
        }
    }
}
