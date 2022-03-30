using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField] private bool _switchTile;

    // lists to hole the various spawn points on a given tile
    [SerializeField] private List<Transform> _mobSpawnPoints;
    [SerializeField] private List<Transform> _itemSpawnPoints;
    [SerializeField] private List<Transform> _hazardSpawnPoints;

    // reference to the player
    private GameObject _player;

    // reference to the player
    private GameObject _levelController;

    // flag if tile has been counted by player object
    private bool _tileCounted;

    // num of tiles is this series of platforms is worth
    [SerializeField] private int _tileValue;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _levelController = GameObject.FindGameObjectWithTag("LevelController");

        SwitchCheck();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnMobs();
        SpawnItems();
        SpawnHazards();
        _tileCounted = false;

        
    }

    // go through the list of spawn points and spawn a mob
    // on each of the points in the list
    private void SpawnMobs()
    {
        foreach (Transform spawnPoint in _mobSpawnPoints)
        {
            // get random mob from list in MoblistController to instantiate
            Transform randomMob = GameObject.Find("MobListController").GetComponent<Spawner>().GetRandom();

            // instantiate the mob onto the spawnpoint position
            Transform mob = Instantiate(randomMob, spawnPoint.position, Quaternion.identity);
        }
    }

    // go through the list of spawn points and spawn an item
    // on each of the points in the list
    private void SpawnItems()
    {
        foreach (Transform spawnPoint in _itemSpawnPoints)
        {
            // get random mob from list in MoblistController to instantiate
            Transform randomMob = GameObject.Find("ItemListController").GetComponent<Spawner>().GetRandom();

            // instantiate the mob onto the spawnpoint position
            Transform item = Instantiate(randomMob, spawnPoint.position, Quaternion.identity);
        }
    }

    // go through the list of spawn points and spawn a hazard
    // on each of the points in the list
    private void SpawnHazards()
    {
        foreach (Transform spawnPoint in _hazardSpawnPoints)
        {
            // get random mob from list in MoblistController to instantiate
            Transform randomMob = GameObject.Find("HazardListController").GetComponent<Spawner>().GetRandom();

            // instantiate the mob onto the spawnpoint position
            Transform hazard = Instantiate(randomMob, spawnPoint.position, Quaternion.identity);
        }
    }

    // check if player has landed on this tile
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if player has landed on this tile and it hasn't been counted
        // increment player tile count
        if (collision.gameObject == _player && _tileCounted == false)
        {
            _tileCounted = true;
            _player.GetComponent<HeroController>().AddTiles(_tileValue);
        }
    }

    // if spawning switch tile check level controller flag and switch the direction
    private void SwitchCheck()
    {        
        if (_switchTile)
        {
            if (_levelController.GetComponent<LevelController>().goingLeft == false)
            {
                _levelController.GetComponent<LevelController>().goingLeft = true;
            }
            else
            {
                _levelController.GetComponent<LevelController>().goingLeft = false;
            }
        }
    }
}
