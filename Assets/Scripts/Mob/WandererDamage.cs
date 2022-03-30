using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class controls the wanderer's passive collision damage
public class WandererDamage : MonoBehaviour
{
    // this mob has passive attacks so details are stored here
    [SerializeField] private float _damage;
    [SerializeField] AudioClip _bouceSFX;
    private AttackDetails _attackDetails;

    // Start is called before the first frame update
    void Start()
    {
        _attackDetails.damage = _damage;
    }

    // if a player bumps into mob then bounce player and do damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // check if collision was made by a player
        if (collision.gameObject.CompareTag("Player"))
        {
            _attackDetails.position = transform.position;

            GetComponentInParent<AudioSource>().PlayOneShot(_bouceSFX);

            // damage player using _damage amount
            collision.transform.SendMessage("Damage", _attackDetails);
        }
    }
}
