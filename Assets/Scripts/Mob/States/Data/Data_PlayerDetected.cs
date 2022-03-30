using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerDetectedStateData", menuName = "Data/State Data/Player Detected State")]

public class Data_PlayerDetected : ScriptableObject
{
    // depending on enemy using this var
    // it could be a charge, spell, or some other action
    public float longRangeActionTime = 1.5f;
}
