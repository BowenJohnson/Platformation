using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// class to hold player high score data from the database to display on scoreboard
public class HighScore : MonoBehaviour
{
    // vars for each field from the database
    [SerializeField] private TMP_Text _username;
    [SerializeField] private TMP_Text _tiles;
    [SerializeField] private TMP_Text _kills;
    [SerializeField] private TMP_Text _treasure;

    // assigns vars to passed in data
    public void NewScoreElement (string _username, int _tiles, int _kills, int _treasure)
    {
        this._username.text = _username;
        this._tiles.text = _tiles.ToString();
        this._kills.text = _kills.ToString();
        this._treasure.text = _treasure.ToString();
    }
}
