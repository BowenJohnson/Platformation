using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLookForPlayerStateData", menuName = "Data/State Data/Look For Player State")]

public class Data_LookForPlayerState : ScriptableObject
{
    // when looking around for player
    // this determines how many times it will
    // turn and look and for how long it looks
    // in that particular direction
    public int numTurns = 2;
    public float turnTime = 0.75f;
}
