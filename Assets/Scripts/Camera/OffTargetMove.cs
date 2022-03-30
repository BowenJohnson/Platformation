using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class moves the camera off target along with the player
public class OffTargetMove : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private void Start()
    {
        this.transform.position = new Vector3(_player.transform.position.x, this.transform.position.y, this.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        this.transform.position = new Vector3(_player.transform.position.x, this.transform.position.y, this.transform.position.z);
    }
}
