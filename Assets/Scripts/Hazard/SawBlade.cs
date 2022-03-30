using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controls the sawblade prefab
public class SawBlade : MonoBehaviour
{
    [SerializeField] private Transform _sawBlade;
    [SerializeField] private float _spinSpeed;
    [SerializeField] private float _damage;
    [SerializeField] private bool _autoKillPlayer;
    [SerializeField] private AudioClip _sfxClip;
    private BasicMobSFX _sfx;
    private AttackDetails _attackDetails;

    private void Start()
    {
        _sfx = GetComponent<BasicMobSFX>();
        _spinSpeed = 500f;
        _attackDetails.damage = _damage;
        _attackDetails.position = _sawBlade.position;
    }

    private void Update()
    {
        _sawBlade.transform.Rotate(new Vector3(0f, 0f, _spinSpeed) * Time.deltaTime);
    }

    // Damage player when it enters the saw collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // check if collision was made by a player
        if (collision.gameObject.CompareTag("Player"))
        {
            // play SFX
            _sfx.PlaySound(_sfxClip);

            // if auto kill is true then kill player on contact
            if (_autoKillPlayer)
            {
                collision.gameObject.GetComponent<HeroController>().KillPlayer();
            }
            // else damage player using _damage amount
            else
            {
                collision.transform.SendMessage("Damage", _attackDetails);
            }       
        }
        // else if a monster hits blade damage monster
        else if (collision.gameObject.CompareTag("Monster"))
        {
            // play SFX
            _sfx.PlaySound(_sfxClip);

            collision.transform.parent.SendMessage("Damage", _attackDetails);
        }
    }
}
