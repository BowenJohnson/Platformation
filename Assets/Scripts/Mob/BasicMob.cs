using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMob : MonoBehaviour
{
    // stats and such for a basic monster
    [SerializeField] protected int _currentHp;
    [SerializeField] protected int _maxHp;
    [SerializeField] protected int _basicDmg;
    [SerializeField] protected float _speed; // 1f is standard
    [SerializeField] protected float _vertKnockbackForce = 1; // how high character gets knocked into air on hit
    [SerializeField] protected float _horizKnockbackForce = 1;    // how far back character gets pushed on hit
    [SerializeField] protected Color _normalColor;
    [SerializeField] protected Color _hurtColor;
    [SerializeField] protected bool _canMove = true;
    [SerializeField] protected bool _knockable = true;

    public void TakeDmg(int dmg)
    {
        // will change mob color to hurt color for
        // passed in duration of coroutine
        StartCoroutine(GotHurt(0.1f));

        _currentHp -= dmg;
        if (_currentHp < 0)
            _currentHp = 0;
    }

    public void HealDmg(int heal)
    {
        _currentHp += heal;
        if (_currentHp > _maxHp)
            _currentHp = _maxHp;
    }

    // knocks mob back when hit
    public void KnockBack(bool hitFromLeft)
    {
        // check if mob base stats says mob can be knocked back
        if (_knockable == true)
        {
            Rigidbody2D _rigbody = GetComponent<Rigidbody2D>();

            StartCoroutine(Stun(1f));

            // stop movement by setting velocity to zero
            _rigbody.velocity = Vector2.zero;

            if (hitFromLeft == true)
            {
                // player hit mob from the right so get knocked left
                _rigbody.velocity = new Vector2(-_horizKnockbackForce , _vertKnockbackForce);
            }
            else
            {
                // player hit mob from left so knocked right
                _rigbody.velocity = new Vector2(_horizKnockbackForce, _vertKnockbackForce);
            }
        }
    }

    IEnumerator GotHurt(float time)
    {
        SpriteRenderer _spriteRend = GetComponent<SpriteRenderer>();
        // change color to pre-set hurt color
        _spriteRend.color = _hurtColor;

        yield return new WaitForSeconds(time);

        // change color back to white
        _spriteRend.color = _normalColor;
    }

    IEnumerator Stun(float time)
    {
        _canMove = false;
        yield return new WaitForSeconds(time);
        _canMove = true;
    }
}
