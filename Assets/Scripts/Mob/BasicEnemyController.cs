using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    private enum State { Moving, Knockback, Dead }
    private State _currState;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private ParticleSystem _particleSystem; // death particle animation

    [SerializeField] private float _groundCheckDist;
    [SerializeField] private float _wallCheckDist;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _knockbackStartTime;
    [SerializeField] private float _knockbackDuration;

    [SerializeField] private Vector2 _knockbackSpeed;

    private GameObject _alive;
    private Rigidbody2D _rb;
    private Animator _anim;

    private bool _groundDetected;
    private bool _wallDetected;

    private int _facingDir;
    private int _damageDir;

    private Vector2 _movement;

    private void Start()
    {
        _alive = transform.Find("Alive").gameObject;
        _rb = _alive.GetComponent<Rigidbody2D>();
        _anim = _alive.GetComponent<Animator>();

        _currentHealth = _maxHealth;

        // start -1 because the sprite is facing that way...
        _facingDir = -1;
    }

    private void Update()
    {
        switch(_currState)
        {
            case State.Moving:
                UpdateMovingState();
                break;

            case State.Knockback:
                UpdateKnockbackState();
                break;

            case State.Dead:
                UpdateDeadState();
                break;
        }
    }

    //--WALKING STATE-----------------------
    private void EnterMovingState()
    {

    }

    private void UpdateMovingState()
    {
        // shoots a ray down from ground check position looking for what is ground layer
        _groundDetected = Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDist, whatIsGround);

        // shoots ray to the right looking for a wall/obstruction
        _wallDetected = Physics2D.Raycast(_wallCheck.position, Vector2.left, _wallCheckDist, whatIsGround);

        if (!_groundDetected || _wallDetected)
        {
            Flip();
        }
        else
        {
            _movement.Set(_moveSpeed * _facingDir, _rb.velocity.y);
            _rb.velocity = _movement;
        }
    }

    private void ExitMovingState()
    {

    }

    //--KNOCKBACK STATE----------------------

    private void EnterKnockbackState()
    {
        // keep track of exact time knockback started
        _knockbackStartTime = Time.time;

        // set the movement to speed times the knockback direction and
        // use knockback speed for the y component
        _movement.Set(_knockbackSpeed.x * _damageDir, _knockbackSpeed.y);

        // change the rigidbody velocity to _movement
        _rb.velocity = _movement;

        // set the animator knockback flag to true
        _anim.SetBool("Knockback", true);
    }

    private void UpdateKnockbackState()
    {
        // if knockback has gone on long enough switch back to moving state
        if (Time.time >= _knockbackStartTime + _knockbackDuration)
        {
            SwitchState(State.Moving);
        }
    }

    private void ExitKnockbackState()
    {
        // when you leave this state return the knockback state to false first
        _anim.SetBool("Knockback", false);
    }

    //--DEAD STATE----------------------

    private void EnterDeadState()
    {
        // spawn chunks and blood...etc.
        // then destroy the parent object
        Instantiate(_particleSystem, _alive.transform.position, _particleSystem.transform.rotation);
        Destroy(gameObject);
    }

    private void UpdateDeadState()
    {

    }

    private void ExitDeadState()
    {

    }

    //--OTHER FUNCTIONS------------------

    // private void Damage(float[] atkDetails)
    public void Damage(float dmg, float atkDir)
    {
        // first var of array is dmg
        // _currentHealth -= atkDetails[0];
        _currentHealth -= dmg;

        // if attack came from the right
        //if (atkDetails[1] > _alive.transform.position.x)
        if (atkDir > _alive.transform.position.x)
        {
            _damageDir = -1;
        }
        // else it came from left
        else
        {
            _damageDir = 1;
        }

        // hit particle

        // flash red for a moment
        StartCoroutine(GotHurt(0.1f));

        // if not dead get knocked back
        if (_currentHealth > 0.0f)
        {
            SwitchState(State.Knockback);
        }
        // if no hp then die
        else if (_currentHealth <= 0.0f)
        {
            SwitchState(State.Dead);
        }
    }

    private void Flip()
    {
        _facingDir *= -1;

        // rotate the sprite 180 degrees so the ray casts will flip direction also
        _alive.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void SwitchState(State state)
    {
        switch (_currState)
        {
            case State.Moving:
                ExitMovingState();
                break;

            case State.Knockback:
                ExitKnockbackState();
                break;

            case State.Dead:
                ExitDeadState();
                break;
        }

        switch (state)
        {
            case State.Moving:
                EnterMovingState();
                break;

            case State.Knockback:
                EnterKnockbackState();
                break;

            case State.Dead:
                EnterDeadState();
                break;
        }

        _currState = state;
    }

    // draw lines so that I can see the raycast positions for
    // ground and wall check in the scene editor
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_groundCheck.position, new Vector2(_groundCheck.position.x, _groundCheck.position.y - _groundCheckDist));
        Gizmos.DrawLine(_wallCheck.position, new Vector2(_wallCheck.position.x + _wallCheckDist, _wallCheck.position.y));
    }

    // change enemy color to red wait an input time then change back
    IEnumerator GotHurt(float time)
    {
        SpriteRenderer _spriteRend = GetComponentInChildren<SpriteRenderer>();

        _spriteRend.color = Color.red;

        yield return new WaitForSeconds(time);

        _spriteRend.color = Color.white;
    }
}