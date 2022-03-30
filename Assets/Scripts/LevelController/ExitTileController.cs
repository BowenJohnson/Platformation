using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// this object is added to an exit tile to transition the player to another scene
public class ExitTileController : MonoBehaviour
{
    // flag when player is in portal area
    private bool _inPortal;

    // flag that exit to next level has started
    private bool _exited;

    // delay time for level fade out
    private float _delayTime = 1f;

    [Header("Destination Scene")]
    // stores the name of the scene the exit leads to
    [SerializeField] private string _exitToScene;

    // player reference
    private GameObject _player;

    // level loader UI reference
    private GameObject _levelLoader;

    [Header("Sound Effects")]
    private BasicMobSFX _sfx;
    [SerializeField] private AudioClip _enterPortalSfx;

    // Start is called before the first frame update
    void Start()
    {
        _inPortal = false;
        _exited = false;
        _player = FindObjectOfType<MageHero>().gameObject;
        _sfx = GetComponent<BasicMobSFX>();

        // put this in a try catch so it doesn't blow up while testing with level loader off
        try
        {
            _levelLoader = FindObjectOfType<LevelLoader>().gameObject;
        }
        catch(Exception ex)
        {
            Debug.Log("Exception: " + ex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if player is in portal range and presses up
        // exit to the _exitToScene
        if (_inPortal)
        {
            if (!_exited && Input.GetAxis("Vertical") > 0)
            {
                _exited = true;
                _sfx.PlaySound(_enterPortalSfx);
                EnterPortal();
            }          
        }
    }

    // if player enters trigger set in portal bool to true
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _inPortal = true;
        }
    }

    // if player leaves trigger set bool to false
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _inPortal = false;
        }
    }

    // stop player from moving, make player invincible, fade to black, and change scenes
    private void EnterPortal()
    {
        _player.GetComponent<MageHero>()._canMove = false;
        _player.GetComponent<MageHero>()._invincible = true;
        SavePlayerState();
        StartCoroutine(DelayExitTransition());
    }

    // saves the player state to the temp static object
    private void SavePlayerState()
    {
        HeroDataTemp.hipPoints = _player.GetComponent<HeroController>().GetCurrentHP();
        HeroDataTemp.mana = _player.GetComponent<HeroController>().GetCurrentMana();
        HeroDataTemp.treasure = _player.GetComponent<HeroController>().ReturnTreasure();
        HeroDataTemp.powerup = _player.GetComponent<HeroController>().GetCurrentPowerupID();
    }

    // delay for number of seconds to allow crossfade animation
    // to finish then reload scene to start over
    IEnumerator DelayExitTransition()
    {
        
        // put this in a try catch so it doesn't blow up while testing with level loader off
        try
        {
            _levelLoader.GetComponent<LevelLoader>().FadeOut();
        }
        catch (Exception ex)
        {
            Debug.Log("Exception: " + ex);
        }

        yield return new WaitForSeconds(_delayTime);
        SceneManager.LoadScene(_exitToScene);
    }
}
