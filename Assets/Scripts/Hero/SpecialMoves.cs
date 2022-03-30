using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this class will control the special move UI images
// and prefab instantiation
public class SpecialMoves : MonoBehaviour
{
    // attack point transform
    [SerializeField] private GameObject _attackPoint;

    // current special prefab
    private GameObject _currentSpecial;

    // current spell buildup FX
    private GameObject _currentBuildUp;

    // reference to the prefab being shot and buildup
    private GameObject _building;
    private GameObject _shooting;

    // keeps track of the mana cost of the current special attack
    public float specialManaCost { get; private set; }

    // mana cost for each special
    [SerializeField] private float _fireballCost;
    [SerializeField] private float _ballLightningCost;
    [SerializeField] private float _iceShardCost;
    [SerializeField] private float _cycloneCost;

    // all special prefabs
    [SerializeField] private GameObject _fireball;
    [SerializeField] private GameObject _ballLightning;
    [SerializeField] private GameObject _iceShard;
    [SerializeField] private GameObject _cyclone;
    [SerializeField] private GameObject _cycloneBuildUp;

    // reference to the UI Image
    [SerializeField] private Image _specialImage;

    // images for the UI
    [SerializeField] private Sprite _fireballImage;
    [SerializeField] private Sprite _ballLightningImage;
    [SerializeField] private Sprite _iceShardImage;
    [SerializeField] private Sprite _cycloneImage;
    [SerializeField] private Sprite _emptyImage;

    // sets current special based on ID input
    public void SetSpecial(int specialID)
    {
        switch(specialID)
        {
            case 1:
                // change UI image
                _specialImage.sprite = _fireballImage;

                // change current special
                _currentSpecial = _fireball;
                _currentBuildUp = null;
                specialManaCost = _fireballCost;

                break;

            case 2:
                // change UI image
                _specialImage.sprite = _ballLightningImage;

                // change current special
                _currentSpecial = _ballLightning;
                _currentBuildUp = null;
                specialManaCost = _ballLightningCost;

                break;

            case 3:
                // change UI image
                _specialImage.sprite = _iceShardImage;

                // change current special
                _currentSpecial = _iceShard;
                _currentBuildUp = null;
                specialManaCost = _iceShardCost;

                break;

            case 4:
                // change UI image and make it visible
                _specialImage.sprite = _cycloneImage;

                // change current special
                _currentSpecial = _cyclone;
                _currentBuildUp = _cycloneBuildUp;
                specialManaCost = _cycloneCost;

                break;

            // this will be in case special is removed
            case 5:
                // change UI image and make it visible
                _specialImage.sprite = _emptyImage;

                // change current special
                _currentSpecial = null;
                specialManaCost = 0;

                break;
        }
    }

    // takes in player location and instantiates current special
    // giving it time for spell effects before releasing
    public void SpawnSpecial()
    {
        if (_currentSpecial != null)
        {
            // deduct mana cost when spell is finished
            GetComponent<HeroController>().LoseMana(specialManaCost);

            // instantiate projectile object on _building position using its state data
            _shooting = Instantiate(_currentSpecial, _building.transform.position, _building.transform.rotation);

            // destroy the buildup animation
            Destroy(_building);

            // lauunch the projectile using unity instantiated values
            _shooting.GetComponent<MagicAttack>().FireProjectile();
        }
    }

    // instantiate the spell build-up animation
    public void SpawnBuildUp()
    {
        _building = Instantiate(_currentBuildUp, _attackPoint.transform.position, _attackPoint.transform.rotation);       
    }

    // destroy the spell build-up animation
    public void DestroyBuildUp()
    {
        Destroy(_building);
    }
}
