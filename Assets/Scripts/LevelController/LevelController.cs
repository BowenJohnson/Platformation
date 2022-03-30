using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Level Controller")]
    // reference to the player
    [SerializeField] protected HeroController _player;

    // flag to switch directions
    public bool goingLeft { get; set; }

    // distance player is to the endposition before spawning another tile
    protected const float _tileSpawnPlayerDist = 30f;

    //reference to the previous tile
    protected Transform _prevTileTransform;

    protected Vector3 _lastEndPosition;

    // reference to the starting tile endpoint
    [SerializeField] protected Transform _startingTile;

    // reference to the current tile prefab
    [SerializeField] protected List<Transform> _platformList;

    protected virtual void Awake()
    {

    }

    protected virtual void Update()
    {
        // if the distance between the player and last position is less
        // than the tile spawn distance then spawn a new tile at the end
        if (Vector3.Distance(_player.transform.position, _lastEndPosition) < _tileSpawnPlayerDist)
        {
            SpawnTile();
        }
    }

    protected virtual void SpawnTile()
    {
        // grabs a random platform from the list and passes it to be spawned
        Transform randomTile = _platformList[Random.Range(0, _platformList.Count)];
        _prevTileTransform = SpawnTile(randomTile, _lastEndPosition);
        _lastEndPosition = _prevTileTransform.Find("EndPosition").position;
    }

    // spawns a new tile based off of the end point of the last tile
    protected virtual Transform SpawnTile(Transform platform, Vector3 spawnPoint)
    {
        Transform nextTile = Instantiate(platform, spawnPoint, Quaternion.identity);
        return nextTile;
    }

    // called when player dies to start player data collection
    public virtual void PlayerDied()
    {
        FindObjectOfType<DataHandler>().GameOver();
    }
}
