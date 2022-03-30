using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpikes : MonoBehaviour
{
    private PolygonCollider2D col;
    private AttackDetails _attackDetails;

    private void Start()
    {
        col = GetComponent<PolygonCollider2D>();
        _attackDetails.damage = 10;
        _attackDetails.position = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // get the player data
        GameObject player = GameObject.FindWithTag("Player");

        // check if collision was made by a player
        if (collision.gameObject == player)
        {
            // play loaded SFX
            GetComponent<AudioSource>().Play();

            // get the hero controller and activate its kill player function
            player.GetComponent<HeroController>().KillPlayer();
        }
        // if monster lands on spikes hurt them
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // play loaded SFX
            GetComponent<AudioSource>().Play();

            collision.transform.parent.SendMessage("Damage", _attackDetails);
        }
    }
}
