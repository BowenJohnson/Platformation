using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// class controls the on screen player UI

public class GUIController : MonoBehaviour
{
    // reference to the player
    private HeroController _player;

    // timer vars
    private float _hpLerpTimer;
    private float _manaLerpTimer;
    [SerializeField] private float _chipSpeed;

    // calculation vars
    private float _fillFront;
    private float _fillBack;
    private float _fillFraction;

    // changing UI images 
    [SerializeField] private Image _currentSpecial;
    [SerializeField] private Image _frontHpBar;
    [SerializeField] private Image _backHpBar;
    [SerializeField] private Image _frontManaBar;
    [SerializeField] private Image _backManaBar;
    [SerializeField] private Image _toggleLeft;
    [SerializeField] private Image _toggleRight;
    [SerializeField] private Sprite _toggleOff;
    [SerializeField] private Sprite _toggleOn;

    // UI text
    [SerializeField] private TextMeshProUGUI _treasureCount;
    [SerializeField] private TextMeshProUGUI _tileCount;

    private void Awake()
    {
        _player = GetComponent<HeroController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _treasureCount.text = _player.ReturnTreasure().ToString();
        _tileCount.text = _player.ReturnTiles().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHpUI();
        UpdateManaUI();
    }

    public void UpdateHpUI()
    {
        _fillFront = _frontHpBar.fillAmount;
        _fillBack = _backHpBar.fillAmount;
        _fillFraction = _player.GetCurrentHP() / _player.GetMaxHP();

        // if fill back is > than fill fraction then damage has been taken
        if (_fillBack > _fillFraction)
        {
            float percentComplete;

            // change back bar to red
            // set front hp bar to current health change
            // adjust percent complete based on lerp timer and chip speed
            // move back bar fill amount
            _backHpBar.color = Color.red;
            _frontHpBar.fillAmount = _fillFraction;
            _hpLerpTimer += Time.deltaTime;
            percentComplete = _hpLerpTimer / _chipSpeed;

            // squaring percentComplete makes the animation speed up as it gets closer to the end
            percentComplete = percentComplete * percentComplete; 
            _backHpBar.fillAmount = Mathf.Lerp(_fillBack, _fillFraction, percentComplete);
        }
        // if fill back is < than fill fraction then healing has happened
        if (_fillFront < _fillFraction)
        {
            float percentComplete;

            // same logic as above just the back bar goes first
            // and the front bar follows based on back bar position
            _backHpBar.color = Color.green;
            _backHpBar.fillAmount = _fillFraction;
            _hpLerpTimer += Time.deltaTime;
            percentComplete = _hpLerpTimer / _chipSpeed;
            percentComplete = percentComplete * percentComplete;
            _frontHpBar.fillAmount = Mathf.Lerp(_fillFront, _backHpBar.fillAmount, percentComplete);
        }
    }

    // same logic as above just adjusting mana bars instead
    public void UpdateManaUI()
    {
        _fillFront = _frontManaBar.fillAmount;
        _fillBack = _backManaBar.fillAmount;
        _fillFraction = _player.GetCurrentMana() / _player.GetMaxMana();

        // if fill back is > than fill fraction then mana has been lost
        if (_fillBack > _fillFraction)
        {
            float percentComplete;

            _backManaBar.color = Color.grey;
            _frontManaBar.fillAmount = _fillFraction;
            _manaLerpTimer += Time.deltaTime;
            percentComplete = _manaLerpTimer / _chipSpeed;
            percentComplete = percentComplete * percentComplete;
            _backManaBar.fillAmount = Mathf.Lerp(_fillBack, _fillFraction, percentComplete);
        }
        // if fill back is < than fill fraction then mana gain has happened
        if (_fillFront < _fillFraction)
        {
            float percentComplete;

            // same logic as above just the back bar goes first
            // and the front bar follows based on back bar position
            _backManaBar.color = Color.green;
            _backManaBar.fillAmount = _fillFraction;
            _manaLerpTimer += Time.deltaTime;
            percentComplete = _manaLerpTimer / _chipSpeed;
            percentComplete = percentComplete * percentComplete;
            _frontManaBar.fillAmount = Mathf.Lerp(_fillFront, _backManaBar.fillAmount, percentComplete);
        }
    }

    // updater functions to be called by the hero controller
    // when player's values change
    public void UpdateTreasure(int treasure)
    {
        _treasureCount.text = treasure.ToString();
    }

    public void UpdateTiles(int tiles)
    {
        _tileCount.text = tiles.ToString();
    }

    // reset Lerp timers to be called by player functions
    // that will add or subtract HP and/or Mana
    public void ResetHpLerp()
    {
        _hpLerpTimer = 0f;
    }
    public void ResetManaLerp()
    {
        _manaLerpTimer = 0f;
    }

    // set the UI to display current special move
    public void SetSpecial(Image specialImage)
    {
        _currentSpecial = specialImage;
    }

    // turning special toggles off and on
    public void ToggleRightOff()
    {
        _toggleRight.sprite = _toggleOff;
    }

    public void ToggleRightOn()
    {
        _toggleRight.sprite = _toggleOn;
    }
    public void ToggleLeftOff()
    {
        _toggleLeft.sprite = _toggleOff;
    }
    public void ToggleLeftOn()
    {
        _toggleLeft.sprite = _toggleOn;
    }
}
