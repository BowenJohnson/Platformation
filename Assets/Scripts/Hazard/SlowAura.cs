using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowAura : HazardAura
{
    [Header("Slow Variables")]
    [SerializeField] protected float _speedReduction;
    [SerializeField] protected float _jumpReduction;
    [SerializeField] protected float _gravityScale;

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
                if (Vector3.Distance(_player.position, _obelisk.position) > _radius)
                {
                    AuraOff();
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
        Instantiate(_particle, _obelisk.position, _particle.transform.rotation, _obelisk);

        // play debuff sound
        _sfx.PlaySound(_debuffSFX);

        _player.SendMessage("ChangeSpeed", -_speedReduction);
        _player.SendMessage("ChangeJumpForce", -_jumpReduction);
        _player.GetComponent<Rigidbody2D>().gravityScale = _gravityScale;
    }

    // turns off aura effects
    public override void AuraOff()
    {
        if (_auraOn)
        {
            // reset player stats to normal
            _player.SendMessage("ChangeSpeed", _speedReduction);
            _player.SendMessage("ChangeJumpForce", _jumpReduction);
            _player.GetComponent<Rigidbody2D>().gravityScale = 1;

            // destroy particle effects if on
            _obelisk.GetComponentInChildren<DestroyParticle>().DestroyThisParticle();
            _player.GetComponentInChildren<DestroyParticle>().DestroyThisParticle();
        }

        _auraOn = false;
    }
}
