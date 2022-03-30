using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaDrainAura : HazardAura
{
    // drain amount, range, and cooldown time
    [Header("Drain Variables")]
    [SerializeField] private float _drainAmount;
    [SerializeField] private float _drainsPerSec;
    private float nextAttackTime = 0f;

    [Header("On Hazard Particle Effect")]
    [SerializeField] private GameObject _auraParticle;

    // reference to object particle is parented to
    [SerializeField] private Transform _particleParent;

    protected override void CheckDist()
    {
        // if obelisk isn't dead check for entry or exit of aura radius
        if (!_obeliskStats._dead)
        {
            if (!_auraOn)
            {
                if (Vector3.Distance(_player.position, _obelisk.position) < _radius)
                {
                    AuraOn();
                }
            }
            else
            {
                // if player leaves range turn off aura
                if (Vector3.Distance(_player.position, _obelisk.position) > _radius)
                {
                    AuraOff();
                }
                // if time >= next attack time then do drain effect
                else
                {
                    if (Time.time >= nextAttackTime)
                    {
                        Drain();

                        // each attack will add 1f to the timer divided by attack rate
                        nextAttackTime = Time.time + 1f / _drainsPerSec;
                    }
                }
            }
        }
    }

    // turns on aura effects
    public override void AuraOn()
    {
        _auraOn = true;

        // this instantiates the particle at the player position
        // with the particle rotation and setting the player as the parent
        Instantiate(_particle, _player.position, _particle.transform.rotation, _player);

        // mimic particle effect on obelisk
        Instantiate(_auraParticle, _obelisk.position, _auraParticle.transform.rotation, _particleParent);
    }

    // turns off aura effects
    public override void AuraOff()
    {
        // if the aura is on remove the particle effects
        if (_auraOn)
        {
            // destroy particle effects
            _obelisk.GetComponentInChildren<DestroyParticle>().DestroyThisParticle();
            _player.GetComponentInChildren<DestroyParticle>().DestroyThisParticle();
        }

        _auraOn = false;
    }

    // drain the player's stat
    private void Drain()
    {
        // play debuff sound
        _sfx.PlaySound(_debuffSFX);

        _player.GetComponent<HeroController>().LoseMana(_drainAmount);
    }
}
