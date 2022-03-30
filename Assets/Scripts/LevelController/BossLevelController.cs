using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System;

public class BossLevelController : MonoBehaviour
{
    // reference to the player
    [Header("Player")]
    [SerializeField] private GameObject _player;

    [Header("Boss")]
    [SerializeField] private GameObject _boss;

    [Header("Evil Wizard")]
    [SerializeField] private GameObject _wizard;
    [SerializeField] private GameObject _teleportParticle;
    [SerializeField] private GameObject _castingParticle;
    [SerializeField] private Transform _castingLoc;

    // boundary walls during fight
    [Header("Boss Platform Walls")]
    [SerializeField] private GameObject _fightWalls;
    [SerializeField] private ParticleSystem _leftWall;
    [SerializeField] private ParticleSystem _rightWall;

    // target for the camera during the fight so it will stay centered
    [Header("Boss Fight Camera Target")]
    [SerializeField] private Transform _cameraTarget;

    // stores the name of the scene the exit leads to
    [Header("Level Restart")]
    [SerializeField] private string _exitToScene;

    // level loader UI reference
    [SerializeField] private GameObject _levelLoader;

    // delay time for level fade out and boss death
    private float _delayTime = 1f;
    private float _bossDeadDelayTime = 6f;
    private float _cameraDelayTime = 3f;

    // camera reference for boss fight adjustment
    [Header("Virtual Camera")]
    [SerializeField] private CinemachineVirtualCamera _vcam;

    // sound effects and controller
    [Header("Cut Scene Sound Effects")]
    private BasicMobSFX _sfx;
    [SerializeField] private AudioClip _shieldsUp;
    [SerializeField] private AudioClip _shieldsDown;
    [SerializeField] private AudioClip _teleport;
    [SerializeField] private AudioClip _wizardCasting;

    // animator reference
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _sfx = GetComponent<BasicMobSFX>();
        _anim = GetComponent<Animator>();

        // check hero temp data
        CheckTempData();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _player)
        {
            // stop player movement
            _player.GetComponent<HeroController>()._canMove = false;

            ChangeCameraFocus();
        }
    }

    public void ActivateShields()
    {
        // play shield activate noise
        _sfx.PlaySound(_shieldsUp);

        // activate walls
        _fightWalls.SetActive(true);

        // turn on particles
        _leftWall.Play();
        _rightWall.Play();

        // deactivate box collider
        GetComponent<BoxCollider2D>().enabled = false;

        StartCoroutine(StartFight());
    }

    // take the boss out of wait state
    public void ActivateBoss()
    {
        // activate boss
        _boss.GetComponent<DemonBoss>().waitOver = true;

        // reactivate player
        _player.GetComponent<HeroController>()._canMove = true;
    }

    // start the delay and deactivate shields
    public void BossDied()
    {
        StartCoroutine(BossDeadDelay());
    }

    // called when player dies to restart level
    public void PlayerDied()
    {
        StartCoroutine(DelayExitTransition());
    }

    // move camera for fight and make wizard start casting shield walls
    public void ChangeCameraFocus()
    {
        // change camera focus to get the whole battle platform
        _vcam.Follow = _cameraTarget.transform;

        _wizard.transform.Rotate(0f, 180f, 0f);
        Instantiate(_castingParticle, _castingLoc.transform.position, _castingLoc.transform.rotation);
        _sfx.PlaySound(_wizardCasting);

        // pause for effect then activate shields
        StartCoroutine(ShieldsDelay());
    }

    // if player temp data isn't null set current player stats
    private void CheckTempData()
    {
        // if temp data hit points aren't at default setting
        // assign those values to the player's current values
        if (HeroDataTemp.hipPoints != 0)
        {
            _player.GetComponent<HeroController>().SetHP(HeroDataTemp.hipPoints);
            _player.GetComponent<HeroController>().SetMana(HeroDataTemp.mana);
            _player.GetComponent<HeroController>().SetTreasure(HeroDataTemp.treasure);
            _player.GetComponent<HeroController>().GetPowerup(HeroDataTemp.powerup);
        }
    }

    // pause for effect then activate shields
    IEnumerator ShieldsDelay()
    {
        yield return new WaitForSeconds(_cameraDelayTime);

        ActivateShields();
    }

    // start wizard teleport effect and then vanish
    IEnumerator StartFight()
    {
        yield return new WaitForSeconds(_cameraDelayTime);

        // instantiate teleport particles
        Instantiate(_teleportParticle, _wizard.transform.position, Quaternion.identity);
        _sfx.PlaySound(_teleport);
        StartCoroutine(WizardVanish());
    }

    // destroy wizard then activate boss 
    IEnumerator WizardVanish()
    {
        Destroy(_wizard);
        yield return new WaitForSeconds(_cameraDelayTime - 2);

        ActivateBoss();
    }

    // wait a few seconds for boss animations then disable shields
    IEnumerator BossDeadDelay()
    {
        yield return new WaitForSeconds(_bossDeadDelayTime);

        // play a shields down sound
        _sfx.PlaySound(_shieldsDown);

        // deactivate walls
        _fightWalls.SetActive(false);

        // turn off particles
        _leftWall.Stop();
        _rightWall.Stop();

        _vcam.Follow = _player.transform;
    }

    IEnumerator DelayExitTransition()
    {
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
