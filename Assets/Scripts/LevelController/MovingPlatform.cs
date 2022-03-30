using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // reference to the player
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
    }

    // check if player or monster has landed on this platform and change parent
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if player or monster has landed on this platform set parent to platform
        if (collision.gameObject == _player)
        {
            collision.collider.transform.SetParent(transform);
        }
        else if(collision.gameObject.tag == "Monster")
        {
            collision.collider.transform.parent.SetParent(transform);
        }
        else if (collision.gameObject.tag == "Wanderer")
        {
            collision.collider.transform.parent.SetParent(transform);
        }
        else if (collision.gameObject.tag == "Arrow")
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    // check if player or monster has landed on this platform and remove parent
    private void OnCollisionExit2D(Collision2D collision)
    {
        // if player or monster has landed on this platform set parent to platform
        if (collision.gameObject == _player)
        {
            collision.collider.transform.SetParent(null);
        }
        else if (collision.gameObject.tag == "Monster")
        {
            collision.collider.transform.parent.SetParent(null);
        }
        else if (collision.gameObject.tag == "Wanderer")
        {
            collision.collider.transform.parent.SetParent(null);
        }
    }
}
