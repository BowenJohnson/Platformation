using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this object controls the respawn tile 
public class RespawnTile : MonoBehaviour
{
    // this tile's respawn location
    [SerializeField] private Transform _respawnLocation;

    // reference to the checkpoint sprite
    [SerializeField] private Checkpoint _checkpoint;

    // flag to prevent saving location on re-entry
    private bool _hasTriggered;

    // audio source reference
    AudioSource _sfx;

    private void Awake()
    {
        _hasTriggered = false;
        _sfx = GetComponent<AudioSource>();
    }

    // on enter if hasn't been triggered set respawn location
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !_hasTriggered)
        {
            _hasTriggered = true;
            SetRespawnLoc();
            _checkpoint.ActivateCheckpoint();
            _sfx.Play();
        }
    }

    // sets the respawn location in the story controller to
    // this tile's respawn location
    private void SetRespawnLoc()
    {
        GameObject.FindObjectOfType<StoryLevelController>()._respawnLoc = _respawnLocation.transform.position;
    }
}
