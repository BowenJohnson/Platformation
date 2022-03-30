using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryLevelController : LevelController
{
    [Header("Story Level Controller")]
    [Header("")]
    [Header("Respawn")]
    // death and respawn Particle
    [SerializeField] private Transform _deathParticle;
    [SerializeField] private Transform _respawnParticle;

    // flag if exit has spawned
    private bool _exitSpawned;

    // respawn variables
    public Vector3 _respawnLoc { get; set; }
    private Vector3 _startPosition;
    private bool _lerping;
    private float _lerpDuration = 3f;
    private float _timePast;
    private float _percentComplete;

    // respawn sound effects
    private BasicMobSFX _sfx;
    [SerializeField] private AudioClip _despawnSFX;
    [SerializeField] private AudioClip _respawntSFX;

    // curve for adjusting lerp speed movement over time
    [SerializeField] private AnimationCurve _lerpSpeed;

    // time delays for respawn
    private float _deathDelay = 3f;
    private float _rezDelay = 1f;
    private float _spriteDelay = 0.5f;

    [Header("Tile Controls")]
    // checkpoint tile
    [SerializeField] private Transform _checkpointTile;

    // exit to next level tile.
    [SerializeField] private Transform _exitTile;

    [SerializeField] private int _checkpointDistance;

    // number of tiles player needs to travel before a portal tile spawns
    [SerializeField] private int _exitDistance;

    private int _tileCount;

    protected override void Awake()
    {
        _tileCount = 0;
        _exitSpawned = false;
        _lerping = false;
        _sfx = GetComponent<BasicMobSFX>();

        // set the last end position to the starting end position
        _lastEndPosition = _startingTile.Find("EndPosition").position;

        // seed the game with 3 tiles to start
        int length = 3;
        for (int i = 0; i < length; i++)
        {
            SpawnTile();
        }
    }

    private void Start()
    {
        // check hero temp data
        CheckTempData();

        _respawnLoc = _player.transform.position;
    }

    protected override void Update()
    {
        if (_lerping)
        {
            Lerping();           
        }

        // if the distance between the player and last position is less
        // than the tile spawn distance and the exit hasn't spawned yet
        // then spawn a new tile at the end
        if (Vector3.Distance(_player.transform.position, _lastEndPosition) < _tileSpawnPlayerDist && !_exitSpawned)
        {
            SpawnTile();
        }
    }

    // spawns the next tile at the last end point
    protected override void SpawnTile()
    {
        // if the tile count equals the checkpoint distance
        // spawn the checkpoint tile
        if (_tileCount == _checkpointDistance)
        {
            _prevTileTransform = SpawnTile(_checkpointTile, _lastEndPosition);
            _lastEndPosition = _prevTileTransform.Find("EndPosition").position;
        }
        // if the tile count equals the exit distance
        // spawn the exit tile
        else if (_tileCount == _exitDistance)
        {
            _prevTileTransform = SpawnTile(_exitTile, _lastEndPosition);
            _lastEndPosition = _prevTileTransform.Find("EndPosition").position;
            _exitSpawned = true;
        }
        // else spawn a random tile from the list
        else
        {
            // grabs a random platform from the list and passes it to be spawned
            Transform randomTile = _platformList[Random.Range(0, _platformList.Count)];
            _prevTileTransform = SpawnTile(randomTile, _lastEndPosition);
            _lastEndPosition = _prevTileTransform.Find("EndPosition").position;
        }

        // increment total tile count
        _tileCount++;
    }

    // spawns a new tile based off of the end point of the last tile
    protected override Transform SpawnTile(Transform platform, Vector3 spawnPoint)
    {
        if(!goingLeft)
        {
            Transform nextTile = Instantiate(platform, spawnPoint, Quaternion.identity);
            return nextTile;
        }
        else
        {
            Transform nextTile = Instantiate(platform, spawnPoint, Quaternion.Euler(0f, -180f, 0f));
            return nextTile;
        }       
    }

    private void DespawnPlayer()
    {
        // wait 3 ish seconds for death anim and etc.
        StartCoroutine(DeathDelay(_deathDelay));     
    }

    // called in the update to interpolate player position
    // to the respawn point over a period of time
    private void Lerping()
    {
        // update timer and completion percentage
        _timePast += Time.deltaTime;
        _percentComplete = _timePast / _lerpDuration;

        // lerp to last respawn point
        // using mathf function to smoothly slow down
        // as it get closer to the end point
        _player.transform.position = Vector3.Lerp(_startPosition, _respawnLoc, Mathf.SmoothStep(0, 1, _percentComplete));

        // if player location equals respawn location
        // respawn player and reset timer and lerp bool
        if (_player.transform.position == _respawnLoc)
        {
            // turn on collider
            _player.gameObject.GetComponent<BoxCollider2D>().enabled = true;

            RespawnPlayer();
            _timePast = 0;
            _lerping = false;
        }
    }

    // moves player to last respawn position
    private void RespawnPlayer()
    {
        StartCoroutine(RespawnDelay(_rezDelay));
    }

    // called when player dies to start respawn at last checkpoint
    public override void PlayerDied()
    {
        // set player to dead layer
        _player.gameObject.layer = LayerMask.NameToLayer("Dead");

        DespawnPlayer();
    }

    // if player temp data isn't null set current player stats
    private void CheckTempData()
    {
        // if temp data hit points aren't at default setting
        // assign those values to the player's current values
        if(HeroDataTemp.hipPoints != 0)
        {
            _player.GetComponent<HeroController>().SetHP(HeroDataTemp.hipPoints);
            _player.GetComponent<HeroController>().SetMana(HeroDataTemp.mana);
            _player.GetComponent<HeroController>().SetTreasure(HeroDataTemp.treasure);
            _player.GetComponent<HeroController>().GetPowerup(HeroDataTemp.powerup);
        }
    }

    // gives a delay for animations and etc. 
    public IEnumerator DeathDelay(float time)
    {        
        yield return new WaitForSeconds(time);

        // set current position
        _startPosition = _player.transform.position;

        // sprite delay for death particle to start
        StartCoroutine(SpriteDealy(_spriteDelay));
    }

    // hiding the player sprite and setting lerp to bool true
    public IEnumerator SpriteDealy(float time)
    {
        // summon death particle
        Instantiate(_deathParticle, _player.transform.position, Quaternion.identity, _player.transform);

        // play despawn sound effects
        _sfx.PlaySound(_despawnSFX);

        yield return new WaitForSeconds(time);

        // disable sprite renderer
        _player.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        // disable box collider
        _player.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        // start lerping
        _lerping = true;
    }

    // gives a delay before reviving the player and giving them control
    public IEnumerator RespawnDelay(float time)
    {
        yield return new WaitForSeconds(time);

        // summon respawn particle
        Instantiate(_respawnParticle, _player.transform.position, _respawnParticle.transform.rotation);

        // play respawn sound effects
        _sfx.PlaySound(_respawntSFX);

        // set layer back to player
        _player.gameObject.layer = LayerMask.NameToLayer("Player");

        // give hp
        _player.HealDmg(5f);     

        // set alive and can move
        _player._isDead = false;
        _player._canMove = true;

        // enable sprite renderer
        _player.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
