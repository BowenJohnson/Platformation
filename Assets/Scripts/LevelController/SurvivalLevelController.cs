using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalLevelController : LevelController
{
    protected override void Awake()
    {
        // set the last end position to the starting end position
        _lastEndPosition = _startingTile.Find("EndPosition").position;

        // seed the game with 3 tiles to start
        int length = 3;
        for (int i = 0; i < length; i++)
        {
            SpawnTile();
        }

        // set user in FirebaseController to user saved from login scene
        FindObjectOfType<FirebaseController>().SetUser(CurrentUserController._currentUser);
    }

    protected override void Update()
    {
        // if the distance between the player and last position is less
        // than the tile spawn distance then spawn a new tile at the end
        if (Vector3.Distance(_player.transform.position, _lastEndPosition) < _tileSpawnPlayerDist)
        {
            SpawnTile();
        }
    }

    protected override void SpawnTile()
    {
        // grabs a random platform from the list and passes it to be spawned
        Transform randomTile = _platformList[Random.Range(0, _platformList.Count)];
        _prevTileTransform = SpawnTile(randomTile, _lastEndPosition);
        _lastEndPosition = _prevTileTransform.Find("EndPosition").position;
    }

    // spawns a new tile based off of the end point of the last tile
    protected override Transform SpawnTile(Transform platform, Vector3 spawnPoint)
    {
        //Transform nextTile = Instantiate(platform, spawnPoint, Quaternion.identity);
        //return nextTile;

        if (!goingLeft)
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

    // called when player dies to start player data collection
    public override void PlayerDied()
    {
        FindObjectOfType<DataHandler>().GameOver();
    }
}
