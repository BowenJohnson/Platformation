using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShardPowerup : BasicCollectable
{
    // elemental particle effect on object
    [SerializeField] private GameObject _fxParticle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // get the player data
        GameObject player = GameObject.FindWithTag("Player");

        // check if collision was made by a player
        if (collision.gameObject == player)
        {
            if (player.GetComponent<HeroController>().GetCurrentPowerupID() != _powerupID)
            {
                // add mana to player total then destroy the potion
                player.GetComponent<HeroController>().GetPowerup(_powerupID);
                DestroyItem();
            }
            // if player already has powerup then get mana value
            // if the player 
            else if (player.GetComponent<HeroController>().GetCurrentMana() < player.GetComponent<HeroController>().GetMaxMana())
            {
                // add mana to player total then destroy the potion
                AddMana();
                DestroyItem();
            }
        }
    }
}
