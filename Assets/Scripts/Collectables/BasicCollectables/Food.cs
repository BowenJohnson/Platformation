using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : BasicCollectable
{
    private CollectableSFX _sfx;
    private float _delayTime;
    private void Start()
    {
        // set values for hp, mana, treasure
        _delayTime = 1f;
        _sfx = GetComponent<CollectableSFX>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // get the player data
        GameObject player = GameObject.FindWithTag("Player");

        // check if collision was made by a player
        if (collision.gameObject == player && !_collected && !player.GetComponent<HeroController>()._isDead)
        {
            // if player doesn't have full health
            if (player.GetComponent<HeroController>().GetCurrentHP() < player.GetComponent<HeroController>().GetMaxHP())
            {
                // set collected flag, play sound FX, add values to player total, then destroy the gem
                _collected = true;
                _sfx.PlaySound();
                AddHP();
                AddMana();
                GetComponent<SpriteRenderer>().enabled = false;
                StartCoroutine(DelayedDestroy(_delayTime));
            }
        }
    }
}
