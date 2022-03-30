using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundWanderer : BasicMob
{
    // *****
    // Ground Wanderer just wanders back and forth on its 
    // platform when it reaches the edge it turns around
    // *****

    // states for animation state machine
    private enum MoveState { idle, walk };
    private MoveState _currState;

    // basic mob base stats and extra stats for gound wanderer
    private Animator _animator;
    public Rigidbody2D _rigbody;
    private RaycastHit2D _groundInfo;
    private bool _movingRight = false;
    private bool _walking = false;
    private bool _falling;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _rayDistance;
    public Transform groundDetection;

    void Start()
    {
        _rigbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentHp = 1;
        _maxHp = 1;
        _basicDmg = 1;
        _speed = 1;
        _vertKnockbackForce = 2;
        _horizKnockbackForce = 2;
        _normalColor = Color.white;
        _hurtColor = Color.red;
        _rayDistance = 1;
        _currState = MoveState.walk;
        _falling = false;
    }

    void Update()
    {
        Move();
        StateSwitch();
        _animator.SetInteger("moveState", (int)_currState);
    }

    // if facing right move right else move left
    private void Move()
    {
        if (!_falling && _canMove)
        {
            if (_animator.speed == 0)
            {
                _animator.speed = 1;
            }

            // if it can move it's walking so set bool
            _walking = true;

            // move the sprite via its transform, speed, and real time
            // transform.Translate(Vector2.left * _speed * Time.deltaTime);
            transform.Translate(_speed * Time.deltaTime * Vector2.left);

            // use the ray caster to check if there is ground ahead to walk on
            _groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, _rayDistance);

            // if there is no ground ahead flip around
            if (_groundInfo.collider == false)
            {
                if (_movingRight == true)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    _movingRight = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    _movingRight = true;
                }
            }
        }
        else
        {
            _animator.speed = 0;
        }

        // falling check so it doesn't keep flipping
        // left and right when not on the ground
        FallingCheck();
    }

    // if moving in a negative y dir then it's falling
    private void FallingCheck()
    {
        // if there is motion in the y direction then its falling
        if (_rigbody.velocity.y < -0.01)
        {
            _falling = true;
        }
        else
        {
            _falling = false;
        }
    }

    // will remove mob sprite and collider while
    // allowing the particle burst death animation
    // before destroying object
    IEnumerator ParticleDeath(float time)
    {
        // set move bool so mob stops moving
        _canMove = false;

        // activate particles
        _particleSystem.Play();

        // turn off box collider and sprite renderer
        // so mob appears to be gone
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        
        // wait timer so particles can blow away before destroying mob
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);       
    }

    // change the animation state
    private void StateSwitch()
    {
        // check if its walking or idle and change state accordingly
        if (_currentHp <= 0)
        {
            StartCoroutine(ParticleDeath(1));
            this.enabled = false;
        }
        if (_walking)
        {
            _currState = MoveState.walk;
        }
        else
        {
            _currState = MoveState.idle;
        }
    }
}
