using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCollectable : MonoBehaviour
{
    // variables used to increase current player stats
    [SerializeField] protected float _hpValue;
    [SerializeField] protected float _manaValue;
    [SerializeField] protected int _treasureValue;
    [SerializeField] protected int _powerupID;
    protected bool _collected = false;

    // set all starting values for the base stats
    public void SetValues(float hp, float mana, int treasure)
    {
        _hpValue = hp;
        _manaValue = mana;
        _treasureValue = treasure;
    }

    // all three get player object then use the public
    // player functions to add the item value to the player
    // current stat values
    public void AddHP()
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<HeroController>().HealDmg(_hpValue);
    }

    public void AddMana()
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<HeroController>().GainMana(_manaValue);
    }

    public void AddTreasure()
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<HeroController>().GetTreasure(_treasureValue);
    }

    public void AddPowerup()
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<HeroController>().GetPowerup(_powerupID);
    }

    // destroys this game object
    // (used after player collects object)
    public void DestroyItem()
    {
        Destroy(this.gameObject);
    }

    // delays destroy based on input to give other components
    // time to finish i.e. animations or SFX
    protected IEnumerator DelayedDestroy(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        DestroyItem();
    }
}
