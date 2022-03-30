using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : BasicCollectable
{
    private CollectableSFX _sfx;
    private float _delayTime;

    private void Start()
    {
        _delayTime = 1f;
        _sfx = GetComponent<CollectableSFX>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // get the player data
        GameObject player = GameObject.FindWithTag("Player");

        // check if collision was made by a player and hasn't been collected yet
        if (collision.gameObject == player && !_collected && !player.GetComponent<HeroController>()._isDead)
        {
            // set collected flag, play sound FX, add values to player total, then destroy the gem
            _collected = true;
            _sfx.PlaySound();
            GetComponent<SpriteRenderer>().enabled = false;
            AddTreasure();
            AddMana();
            StartCoroutine(DelayedDestroy(_delayTime));
        }
    }
}
