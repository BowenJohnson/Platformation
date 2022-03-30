using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class gathers player metrics to send to the
// game over screen and the database for high score records
public class DataHandler : MonoBehaviour
{
    // reference to the player and game over UI to collect data
    private HeroController _heroData;
    [SerializeField] private GameObject _ui;

    // reference to the firebase controller
    private FirebaseController _fbControler;

    // flag to track if data has been sent
    public bool _dataSent { get; private set; }

    // properties to store the player metrics
    public int tilestTraveled { get; private set; }
    public int treasureCollected { get; private set; }
    public int mobsKilled { get; set; }

    private void Awake()
    {
        // grab the reference data
        _heroData = FindObjectOfType<HeroController>();
        _fbControler = FindObjectOfType<FirebaseController>();
    }

    private void Start()
    {
        _dataSent = false;
        tilestTraveled = 0;
        treasureCollected = 0;
        mobsKilled = 0;

        // get values from database one time
        // so that there isn't a need to constantly ping the database for checks
        _fbControler.GetDbSnapshot();
    }

    // set the data to be sent from player data
    private void RetrievePlayerData()
    {
        tilestTraveled = _heroData.ReturnTiles();
        treasureCollected = _heroData.ReturnTreasure();
        mobsKilled = _heroData.GetMobsKilled();
    }

    // send player data to the game over UI
    // then send the data over to the database
    public void GameOver()
    {
        // if data hasn't been sent set flag then
        // retrieve player data, activate the UI with current data
        // and send data
        if (!_dataSent)
        {
            _dataSent = true;
            RetrievePlayerData();
            _ui.GetComponent<GameOverUI>().SetValues(tilestTraveled, treasureCollected, mobsKilled);
            _ui.GetComponent<GameOverUI>().EnableUI();

            // if curr score > database high score then update database
            if (HighScore())
            {
                // send updates to the db
                _fbControler.SaveToDatabase(tilestTraveled, mobsKilled, treasureCollected);
            }
        }
    }

    // sets high score based on hierarchy of local database values
    private bool HighScore()
    {
        // if less tiles traveled no high score return false
        if (tilestTraveled <= CurrentUserController.tiles)
        {
            return false;
        }
        // if tiles traveled greater than db return true
        else if (tilestTraveled > CurrentUserController.tiles)
        {
            return true;
        }
        // if tiles traveled same as db but kills greater return true
        else if (tilestTraveled == CurrentUserController.tiles && mobsKilled > CurrentUserController.kills)
        {
            return true;
        }
        // if tiles traveled and kills equal db but treasure count is greater return true
        else if (tilestTraveled == CurrentUserController.tiles && mobsKilled == CurrentUserController.kills && treasureCollected > CurrentUserController.treasure)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
