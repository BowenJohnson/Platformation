using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class controls the cyclone special attack projectile
public class Cyclone : MagicAttack
{
    private Animator _animator;
    private ProjectileSFX _sfx;
    private bool _fizzle;

    private void Start()
    {
        // cache references
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRend = GetComponentInChildren<SpriteRenderer>();
        _sfx = GetComponent<ProjectileSFX>();

        // initially make uneffected by gravity and set velocity
        _rb.gravityScale = 0.0f;
        _rb.velocity = transform.right * _speed;

        // set starting location
        _xStartPosition = transform.position.x;

        // start playing cyclone SFX
        _sfx.PlayShoot();
    }

    private void Update()
    {
        _attackDetails.position = transform.position;
    }

    private void FixedUpdate()
    {
        CheckDist();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_fizzle)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Wanderer"))
            {
                collision.transform.parent.SendMessage("Damage", _attackDetails);
            }          
        }
    }

    // check to see if cyclone is at max range then dissipate
    private void CheckDist()
    {
        // if max range has been reached instantiate fizzle partical and destroy object
        if (Mathf.Abs(_xStartPosition - transform.position.x) >= _travDist)
        {
            if (_fizzle == false)
            {
                // turn on the fizzle flag to stop collisions
                _fizzle = true;

                // change to dissipate animation (state 1)
                // it will destroy itself at end of animation
                _animator.SetInteger("state", 1);
            }
        }
    }
}
