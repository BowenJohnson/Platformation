using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardAura : MonoBehaviour
{
    // save player data as variable so it can be used when object is destroyed
    // to revert player speed
    protected Transform _player;

    [Header("Hazard Aura Variables")]
    // save obelisk position to add particle effect
    [SerializeField] protected Transform _obelisk;
    [SerializeField] protected BasicObelisk _obeliskStats;

    // distance for debuff to activate
    [SerializeField] protected float _radius;

    // bool to track if player has exited so it wont
    // get called twice like it wants to for some reason...
    protected bool _exited;

    protected bool _auraOn;

    // save the _particle effect as variable to be added in Unity
    [SerializeField] protected GameObject _particle;
    
    // audio source reference
    protected BasicMobSFX _sfx;

    // debuff SFX
    [SerializeField] protected AudioClip _debuffSFX;

    protected virtual void Awake()
    {
        _player = FindObjectOfType<HeroController>().transform;
    }

    protected virtual void Start()
    {
        _sfx = GetComponentInParent<BasicMobSFX>();
    }

    protected virtual void Update()
    {
        CheckDist();
    }

    protected virtual void CheckDist()
    {

    }

    // turns on aura effects
    public virtual void AuraOn()
    {

    }

    // turns off aura effects
    public virtual void AuraOff()
    {

    }

    // draws effect range as a sphere in unity
    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
