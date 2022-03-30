using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class sets the hero data temp at start of game
public class HeroDataStart : MonoBehaviour
{
    private void Start()
    {
        // set temp hero data
        HeroDataTemp.hipPoints = 0;
        HeroDataTemp.mana = 0;
        HeroDataTemp.treasure = 0;
        HeroDataTemp.powerup = 0;
    }
}
