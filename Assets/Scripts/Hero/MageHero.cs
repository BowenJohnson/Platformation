using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageHero : HeroController
{
	// states for animation state machine
	private enum MoveState
	{
		idle, runnging, jumping, falling, dash, attack, skill1,
		skill2, skill3, hurt, die, doubleJump, wallSlide, jumpAttack, shoot,
		fireball, ballLightning, iceShard, cyclone 
	}

	//private Rigidbody2D _rigbdy;
	private MoveState _state = MoveState.idle;
	private Animator _animator;
	private float _movement;

	private bool _facingLeft;
	private bool _basicAttack;
	private bool _basicRngAttack;
	private bool _specialAttack;
	private bool _grounded;
	private bool _firstGround; // prevents sfx at game start and respawn

	// reference to SFX player
	private EntitySFX _sfx;

	// referencing the object under magehero in unity
	[Header("Attack")]
	[SerializeField] private Transform _attackPoint;
	[SerializeField] private float _attackRange = .5f;
	[SerializeField] private LayerMask _enemyLayers;

	// attack time delay vars
	[SerializeField] private float attackRate = 1f;
	private float nextAttackTime = 0f;

	// basic attack 2nd animation stuff
	[SerializeField] private GameObject _basicAtkPrefab;

	// basic ranged attack prefab
	[Header("Ranged Attack")]
	[SerializeField] private GameObject _basicRngPrefab;
	private GameObject _basicRanged;

	// basic ranged attack vars
	[SerializeField] private float _rngManaCost;
	[SerializeField] private float _rngSpeed;
	[SerializeField] private float _rngDist;
	[SerializeField] private float _rngDmg;

	// checks the ground
	[Header("Ground Check")]
	[SerializeField] private Transform _groundCheck;

	// layer masks for targeting
	[SerializeField] private LayerMask whatIsGround;

	// size of ground check sphere
	private float groundCheckRadi = 0.17f;

	private void Awake()
    {
		// set bools
		_isDead = false;
		_grounded = true;
		_firstGround = false;
		
		_invincible = false;
		_basicAttack = false;
		_basicRngAttack = false;
		_specialAttack = false;

		// get references
		_rigbdy = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
		_sfx = GetComponent<EntitySFX>();
		_specialMoves = GetComponent<SpecialMoves>();
	}

    private void Start()
	{
        _canMove = true;
	}

	private void Update()
	{
		// if game is not paused check for player inputs
		if (!PauseMenu.gamePaused)
		{
			// checks ground
			_grounded = CheckGround();

			// if player has fallen down too far then they die
			if (_rigbdy.transform.position.y < -10f)
			{
				KillPlayer();
			}
			// if player isn't hurt then player can move
			else if (_state != MoveState.hurt)
			{
				_movement = Input.GetAxis("Horizontal");
				Move();
				CheckInputs();
			}
			StateChange();
			_animator.SetInteger("state", (int)_state);
		}
	}

    private void Move()
	{
		// check to see if I can move flag is true
		if (_canMove)
		{
			transform.position += _moveSpeed * Time.deltaTime * new Vector3(_movement, 0, 0);

			// if moving left flip the sprite left if right flip right
			if (_movement < 0)
			{
				// if using a special break animation and start running
				if ((int)_state > 14)
				{
					BreakSpecialAnim("run");
				}

				// this will rotate the whole sprite instead of flipping the image
				// this way the fire point for spells and etc will also point
				// the same direction the player is looking
				if (!_facingLeft)
                {
					_facingLeft = true;
					transform.Rotate(0f, 180f, 0f);
                }
			}
			else if (_movement > 0)
			{
				// if using a special break animation and start running
				if ((int)_state > 14)
				{
					BreakSpecialAnim("run");
				}

				if (_facingLeft)
				{
					_facingLeft = false;
					transform.Rotate(0f, 180f, 0f);
				}
			}
		}
	}

	// track when player has hit ground after being knocked in the air and etc.
	public virtual bool CheckGround()
	{
		return Physics2D.OverlapCircle(_groundCheck.position, groundCheckRadi, whatIsGround);
	}

	// check for player button inputs
	private void CheckInputs()
    {
		if (_canMove)
        {
			if (Input.GetButtonDown("Jump") && _grounded)
			{
				Jump();
			}

			// if the jump button is released early and the character
			// is still moving up slow the jump velocity by half
			if (Input.GetButtonUp("Jump") && _rigbdy.velocity.y > 0)
            {
				_rigbdy.velocity = new Vector2(_rigbdy.velocity.x, _rigbdy.velocity.y * .5f);
            }

			// check attack speed timer to see if you can attack again
			if (Time.time >= nextAttackTime)
			{
				// check for basic attack input
				if (Input.GetButtonDown("Fire1"))
				{
					// break special anim if in motion
					BreakCheckSpecial();

					// set animation bool
					_basicAttack = true;

					// each attack will add 1f to the timer divided by attack rate
					nextAttackTime = Time.time + 1f / attackRate;
				}
				// check for basic ranged attack input
				else if (Input.GetButtonDown("Fire2"))
				{
					// if player has enough mana
					if (_currentMana >= _rngManaCost)
					{
						// break special anim if in motion
						BreakCheckSpecial();

						// deduct mana cost
						LoseMana(_rngManaCost);

						// set animation bool
						_basicRngAttack = true;

						// each attack will add 1f to the timer divided by attack rate
						nextAttackTime = Time.time + 1f / attackRate;
					}
				}
				// check for special attack input
				else if (Input.GetButtonDown("Fire3"))
				{
					// check if player has a special attack
					if (_specialID != 0)
					{
						// if player has enough mana
						if (_currentMana >= _specialMoves.specialManaCost && !_specialAttack)
						{
							// set animation bool
							_specialAttack = true;

							// each attack will add 1f to the timer divided by attack rate
							nextAttackTime = Time.time + 1f / attackRate;							
						}
					}
				}
			}
		}
    }

	private void Jump()
	{
		// add force to the rigidbody and adjust state related bools
		_rigbdy.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
		_state = MoveState.jumping;


		// play jump SFX
		_sfx.PlayClip("jump");

		//_jumping = true;
		_grounded = false;
	}

	private void BasicAttack()
    {
		// play prebasic attack SFX
		_sfx.PlayClip("basicAttack");

		// start 2nd animation
		BasicAtkShoot();

		// assign the damage and position to be passed into
		// the mob for damage assignment
		_attackDetails.damage = _baseDmg;
		_attackDetails.position = transform.position;
		_attackDetails.stunDmg = _stunDmg;

		// detect enemies in range:
		// creates a circle at attackPoint with radius
		// of attackRange and targets everything in enemyLayers
		// saves all of the targets in an array
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayers);

		// damage them:
		foreach(Collider2D enemy in hitEnemies)
        {
			// call the damage function in enemies and pass in attack details
			enemy.transform.parent.SendMessage("Damage", _attackDetails);
		}
	}

	// fire the character's basic ranged attack
	private void BasicRangedAttack()
    {

		// instantiate projectile object on attack position using its state data 
		_basicRanged = Instantiate(_basicRngPrefab, _attackPoint.position, _attackPoint.rotation);

		// call fire function from script
		_basicRanged.GetComponent<MagicAttack>().FireProjectile(_rngSpeed, _rngDist, _rngDmg);
	}

	// instantiate another prefab animation to attack with
    private void BasicAtkShoot()
    {
        Instantiate(_basicAtkPrefab, _attackPoint.position, _attackPoint.rotation, _attackPoint);
	}

    // for testing attackrange by drawing the hitbox in scene view
    private void OnDrawGizmos()
	{
		if (_attackPoint == null)
			return;

		// draws a circle with its center at attackPoint and radius of attackRange
		Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);

		Gizmos.DrawWireSphere(_groundCheck.position, groundCheckRadi);
	}

	// this bool is switched in the animator so it doesn't
	// keep playing the attack animation
	private void BasicAttackOff()
    {
		_basicAttack = false;
	}

	private void RangedAttackOff()
    {
		_basicRngAttack = false;
    }

	private void SpecialAttackOff()
	{
		_specialAttack = false;
	}

	// checking for collisions with the ground
	private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.CompareTag("Ground"))
        {
			// prevents sfx from playing when level starts
			// and player makes initial contact when grounded
			// makes first detection
			if (_firstGround)
			{
				_grounded = true;
				_sfx.PlayClip("land");
			}
            else
            {
				_firstGround = true;
            }
		}
    }

	// checks if special is being cast and stops it
	private void BreakCheckSpecial()
    {
		// if using a special break animation
		if ((int)_state > 14)
		{
			_specialAttack = false;
		}
	}

	// breaks the special attack animation and replaces
	// it with run
	private void BreakSpecialAnim(string anim)
    {
		_specialAttack = false;
		_animator.Play(anim);
	}

	// plays player special SFX from animator
	public void PlayerSpecialSFX()
    {
		_sfx.PlayClip("special");
	}

	// called in the player animation to play player
	// death sound effects
	public void PlayerDeathSFX()
    {
		_sfx.PlayClip("death");
    }

	// sets the special ability from the player temp data
	public void SpecialFromTemp(int special)
    {
		GetPowerup(special);
	}

	// changing the various states for the animation
	private void StateChange()
	{
		// check if player is dead
		if(_currentHp == 0)
        {
			// set player can move flag to false
			if (_isDead == false)
			{
				_isDead = true;
				_canMove = false;
				_firstGround = false;
				_state = MoveState.die;
				PlayerDead();
			}
        }
		// check if player is hurt
		else if (_state == MoveState.hurt)
        {
			// stop attacking if doing so
			_basicAttack = false;
        }
		else if (_basicAttack)
        {
			// set animation state
			_state = MoveState.attack;
		}
        else if (_basicRngAttack)
        {
            _state = MoveState.shoot;

        }
		else if (_specialAttack)
        {
			_state = MoveState.cyclone;
        }
        // if there is any velocity in the y direction stay in jumping state
        else if (Mathf.Abs(_rigbdy.velocity.y) > 0)
		{
			_state = MoveState.jumping;
		}
		else if (_movement != 0 && _canMove)
		{
			_state = MoveState.runnging;
		}
		else
		{
			_state = MoveState.idle;
		}
	}
}
