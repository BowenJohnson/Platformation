using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// base class that all hero characters will inherit
public class HeroController : MonoBehaviour
{
	// serialized fields will be displayed in Unity
	[Header("Base Stats")]
	[SerializeField] protected float _currentHp;
	[SerializeField] protected float _maxHp;
	[SerializeField] protected float _currentMana;
	[SerializeField] protected float _maxMana;
	[SerializeField] protected int _specialID;              // current powerup ID
    public bool _isDead { get; set; }

    [Header("Movement")]
	[SerializeField] protected float _moveSpeed;			// horizontal velocity
    [SerializeField] protected float _jumpForce;            // vertical velocity of jump force

	[Header("Damage")]
	[SerializeField] protected float _baseDmg;				// how much HP damage the player does
	[SerializeField] protected float _stunDmg;              // how much stun damage the player does

	[Header("Knockback")]
	[SerializeField] protected float _vertKnockbackForce;	// how high character gets knocked into air on hit
	[SerializeField] protected float _horizKnockbackForce;  // how far back character gets pushed on hit
	public bool _canMove { get; set; }                      // flag to stop movement
	public  bool _invincible { get; set; }					// flag to stop taking damage

	protected int _mobsKilled;								// current num of mobs killed
    protected int _treasure;								// current treasure count
	protected int _tiles;                                   // current tiles traveled

	protected Rigidbody2D _rigbdy;							// reference to the player's rigidbody
	protected AttackDetails _attackDetails;                 // struct that holds HP damage, stun damage, and attack direction
	protected SpecialMoves _specialMoves;					// reference to the player's special moves controller

	public void TakeDmg(float dmg)
	{
		_currentHp -= dmg;
		if (_currentHp < 0)
			_currentHp = 0;
	}

	public void Damage(AttackDetails attackDetails)
    {
		// reset the Lerp timer in the GUI controller
		GetComponent<GUIController>().ResetHpLerp();

		_currentHp -= attackDetails.damage;
		if (_currentHp < 0)
			_currentHp = 0;

		KnockBack(attackDetails.position);
	}

    // knock player back based on hit direction
    public void KnockBack(Vector2 position)
    {
        if (position.x > _rigbdy.position.x)
        {

            // enemy is to the right so knock player back left that's why first arg is negative
            _rigbdy.velocity = new Vector2(-_horizKnockbackForce, _vertKnockbackForce);
            StartCoroutine(Stun(.5f));
            StartCoroutine(MakeInvincible(1f));
        }
        else
        {
            // enemy is to the left so knock player right
            _rigbdy.velocity = new Vector2(_horizKnockbackForce, _vertKnockbackForce);
            StartCoroutine(Stun(.5f));
            StartCoroutine(MakeInvincible(1f));
        }
    }

    public void HealDmg(float heal)
	{
		// reset the Lerp timer in the GUI controller
		GetComponent<GUIController>().ResetHpLerp();

		_currentHp += heal;
		if (_currentHp > _maxHp)
			_currentHp = _maxHp;
	}

	public void SetHP(float hp)
    {
		_currentHp = hp;
    }

	public void LoseMana(float lost)
	{
		// reset the Lerp timer in the GUI controller
		GetComponent<GUIController>().ResetManaLerp();

		_currentMana -= lost;
		if (_currentMana < 0)
			_currentMana = 0;
	}

	public void GainMana(float gained)
	{
		// reset the Lerp timer in the GUI controller
		GetComponent<GUIController>().ResetManaLerp();

		_currentMana += gained;
		if (_currentMana > _maxMana)
			_currentMana = _maxMana;
	}

	public void SetMana(float mana)
    {
		_currentMana = mana;
    }

    public void GetTreasure(int treasure)
    {
        _treasure += treasure;

        // pass the new value onto the GUI to be updated
        GetComponent<GUIController>().UpdateTreasure(_treasure);
    }

	public void SetTreasure(int treasure)
    {
		_treasure = treasure;
    }

    public void LoseTreasure(int treasure)
    {
        _treasure -= treasure;

        // pass the new value onto the GUI to be updated
        GetComponent<GUIController>().UpdateTreasure(_treasure);
    }

    // returns current treasure value for level start in GUI
    public int ReturnTreasure()
    {
        return _treasure;
    }

    // increases current tile count
    public void AddTiles(int tile)
	{
		_tiles += tile;

		// pass the new value onto the GUI to be updated
		GetComponent<GUIController>().UpdateTiles(_tiles);
	}

	// returns current tile value for level start in GUI
	public int ReturnTiles()
	{
		return _tiles;
	}

	// instantly kills the player
	public void KillPlayer()
    {
		_currentHp = 0;
    }

	// calls player died function in level controller
	// so it can either respawn player or pass metrics
	// to the database depending on game mode
	public void PlayerDead()
    {
		_isDead = true;
		_canMove = false;

		GameObject levelController = GameObject.FindGameObjectWithTag("LevelController");
		levelController.SendMessage("PlayerDied");	
	}
	
	// change move and jump forces for buffs/debuffs
	public void ChangeSpeed(float newSpeed)
    {
		_moveSpeed += newSpeed;

		if (_moveSpeed < 0)
			_moveSpeed = 0;
    }

	public void ChangeJumpForce(float newForce)
	{
		_jumpForce += newForce;

		if (_jumpForce < 0)
			_jumpForce = 0;
	}

	// get current and max HP and Mana for the GUI
	public float GetCurrentHP()
    {
		return _currentHp;
    }

	public float GetMaxHP()
    {
		return _maxHp;
    }

	public float GetCurrentMana()
	{
		return _currentMana;
	}

	public float GetMaxMana()
	{
		return _maxMana;
	}

	// set current power-up ID
	public void GetPowerup(int _powerupID)
    {
		_specialID = _powerupID;
		_specialMoves.SetSpecial(_powerupID);
    }

	public int GetCurrentPowerupID()
    {
		return _specialID;
    }

	public void AddMobsKilled()
    {
		_mobsKilled++;
    }

	public int GetMobsKilled()
    {
		return _mobsKilled;
    }

	// timers for starting and stopping effects
	public IEnumerator Stun(float time)
	{
		_canMove = false;
		yield return new WaitForSeconds(time);

		if (!_isDead)
		{
			_canMove = true;
		}
	}
	public IEnumerator MakeInvincible(float time)
	{
		_invincible = true;
		yield return new WaitForSeconds(time);
		_invincible = false;
	}
}
