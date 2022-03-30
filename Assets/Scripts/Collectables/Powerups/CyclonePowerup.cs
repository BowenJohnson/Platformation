using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclonePowerup : BasicCollectable
{
    // elemental particle effect on object
    [SerializeField] private GameObject _fxParticle;
    private Renderer[] _renderers;
    private CollectableSFX _sfx;
    private float _delayTime = 1;

    private void Start()
    {
        _renderers = gameObject.GetComponentsInChildren<Renderer>();
        _sfx = GetComponent<CollectableSFX>();
        _collected = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // get the player data
        GameObject player = GameObject.FindWithTag("Player");

        // check if collision was made by a player
        if (collision.gameObject == player && !_collected)
        {
            if (player.GetComponent<HeroController>().GetCurrentPowerupID() != _powerupID)
            {
                // set collected, play SFX, add mana to player total
                _collected = true;
                _sfx.PlaySound();
                player.GetComponent<HeroController>().GetPowerup(_powerupID);

                // turn off all child sprites
                for (int i = 0; i < _renderers.Length; i++)
                {
                    _renderers[i].enabled = false;
                }

                // destroy object off screen after SFX has time to play
                StartCoroutine(DelayedDestroy(_delayTime));
            }
            // if player already has powerup then get mana value
            // if the player 
            else if (player.GetComponent<HeroController>().GetCurrentMana() < player.GetComponent<HeroController>().GetMaxMana())
            {
                // set collected, play SFX, add mana to player total
                _collected = true;
                _sfx.PlaySound();
                AddMana();

                // turn off all child sprites
                for (int i = 0; i < _renderers.Length; i++)
                {
                    _renderers[i].enabled = false;
                }

                // destroy object off screen after SFX has time to play
                StartCoroutine(DelayedDestroy(_delayTime));
            }
        }
    }
}
