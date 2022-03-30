using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicObelisk : MonoBehaviour
{
    [Header("Base Stats and Transforms")]
    [SerializeField] private float _currentHp;
    [SerializeField] private float _hoverSpeed;
    [SerializeField] private Transform _obelisk;
    [SerializeField] private Transform _aura;
    [SerializeField] private Transform _hitChunk;
    [SerializeField] private Transform _smallChunk;
    [SerializeField] private Transform _bigChunk;
    public bool _dead { get; set; }

    // min and max points that it will hover between
    [Header("Hover Distances")]
    [SerializeField] private Transform _hoverMax;
    [SerializeField] private Transform _hoverMin;
    private Vector3 _nextPosition;

    // death delay time
    private float _delayTime;

    // audio source reference
    BasicMobSFX _sfx;

    // hit and death SFX
    [Header("Sound Effects")]
    [SerializeField] AudioClip _hitSFX;
    [SerializeField] AudioClip _deathSFX;

    // get audio script reference, set next position,
    // and death delay time
    private void Start()
    {
        _sfx = GetComponent<BasicMobSFX>();
        _nextPosition = _hoverMax.position;
        _delayTime = 5;
        _dead = false;
    }

    // Update is called once per frame
    private void Update()
    {
        Hover();
    }

    // move the obelisk's position between the min and max positions
    private void Hover()
    {
        _obelisk.position = Vector3.MoveTowards(_obelisk.position, _nextPosition, _hoverSpeed * Time.deltaTime);

        // if distance to next position is 0.1 or less change direction
        if (Vector3.Distance(_obelisk.position, _nextPosition) <= 0.1)
        {
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        // if next position does not equal current position then change to other position
        _nextPosition = _nextPosition != _hoverMax.position ? _hoverMax.position : _hoverMin.position;
    }

    // function in all destructable game objects to take damage
    private void Damage(AttackDetails attackDetails)
    {
        if (!_dead)
        {
            // summon hit particle
            Instantiate(_hitChunk, transform.position, _hitChunk.transform.rotation);
            _currentHp -= attackDetails.damage;

            // if out of hp then die
            if (_currentHp <= 0)
            {
                Death();
            }
            else
            {
                // play hit sound effects here so it doesn't
                // delay the death shatter sounds
                _sfx.PlaySound(_hitSFX);
            }
        }
    }
    
    // steps to take for a smooth death animation
    private void Death()
    {
        // set dead bool so animation won't repeat on hit
        _dead = true;

        // disable aura
        GetComponentInChildren<HazardAura>().AuraOff();

        // disable sprite renderer
        GetComponentInChildren<SpriteRenderer>().enabled = false;

        // play death sound
        _sfx.PlaySound(_deathSFX);

        // spawn death particles
        Instantiate(_smallChunk, transform.position, _hitChunk.transform.rotation);
        Instantiate(_bigChunk, transform.position, _hitChunk.transform.rotation);

        // start delayed death timer
        StartCoroutine(DelayedDestroy(_delayTime));
    }

    // delay destroying object
    public IEnumerator DelayedDestroy(float _delayTime)
    {
        yield return new WaitForSeconds(_delayTime);
        Destroy(this.gameObject);
    }
}
