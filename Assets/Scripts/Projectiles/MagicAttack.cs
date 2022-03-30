using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    protected SpriteRenderer _spriteRend;
    protected AttackDetails _attackDetails;

    [Header("Magic Attack Stats")]
    // speed of projectile
    [SerializeField] protected float _speed;

    // how far has traveled
    [SerializeField] protected float _travDist;

    protected float _xStartPosition;

    [SerializeField] protected float _dmg;

    [SerializeField] protected float _dmgRadius;

    // time before object is destroyed after hitting ground
    [SerializeField] protected float _destroyTime;

    // flag to track if projectile has hit a platform
    protected bool _hitGround;

    protected Rigidbody2D _rb;

    [SerializeField] protected LayerMask _whatIsGround;
    [SerializeField] protected LayerMask _whatIsEnemy;

    [Header("Magic Attack Dmg Tansform")]
    [SerializeField] protected Transform _dmgPos;
    protected float _fizzleDelayTime = .25f;

    // function that is called when projectile is instantiated
    public void FireProjectile(float speed, float travDist, float dmg)
    {
        _speed = speed;
        _travDist = travDist;
        _attackDetails.damage = dmg;
    }

    public void FireProjectile()
    {
        _attackDetails.damage = _dmg;
    }

    protected IEnumerator DelayedDestroy(float _destroyTime)
    {
        yield return new WaitForSeconds(_destroyTime);
        Destroy(gameObject);
    }

    protected IEnumerator FizzleOut(float _destroyTime)
    {
        // make sprite dissapear so all you can see is fizzle out particles
        _spriteRend.enabled = false;
        yield return new WaitForSeconds(_destroyTime);
        Destroy(gameObject);
    }

    protected IEnumerator FizzleDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_dmgPos.position, _dmgRadius);
    }
}
