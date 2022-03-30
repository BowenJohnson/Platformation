using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonFireball : MagicAttack
{
    [Header("Demon Fireball Attack Transforms")]
    [SerializeField] private GameObject _hitParticle;
    private Animator _anim;
    private ProjectileSFX _sfx;
    private bool _hitSomething;
    private bool _fizzle;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRend = GetComponentInChildren<SpriteRenderer>();
        _sfx = GetComponent<ProjectileSFX>();
        _anim = GetComponentInChildren<Animator>();

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

    // Update is called once per frame
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

            dmgHit.transform.SendMessage("Damage", _attackDetails);

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

                // set animation to fizzle
                _anim.SetBool("fizzle", true);
            }
        }
    }
}
