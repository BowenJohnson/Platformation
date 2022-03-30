using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMoveStateData", menuName = "Data/State Data/Move State")]

// storing all variables related to MoveState
public class Data_MoveState : ScriptableObject
{
    public float movementSpeed = 0.75f;
}
