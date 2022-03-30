using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // list of transforms that can be spawned
    [SerializeField] private List<Transform> _spawnList;

    // function called by other objects to get a random
    // mob/item/whatever from its list
    public Transform GetRandom()
    {
        Transform randomTransform = _spawnList[Random.Range(0, _spawnList.Count)];
        return randomTransform;
    }
}
