using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// this object controls the camera's lower boundary
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _offTarget;
    private CinemachineVirtualCamera _vcam;
    private bool _inBounds;
    private float _lowerBound = -4f;

    // Start is called before the first frame update
    void Start()
    {
        _inBounds = true;
        _vcam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        BoundsCheck();
    }

    // stop following player if player goes down too far
    public void BoundsCheck()
    {
        // if player is in bounds and goes out set out of bounds
        // and follow camera off target
        if (_inBounds)
        {
            if (_player.transform.position.y < _lowerBound)
            {
                _inBounds = false;
                _vcam.Follow = _offTarget.transform;
            }
        }
        // if player is out of bounds and comes in
        // set to in bounds and follow player
        else
        {
            if (_player.transform.position.y >= _lowerBound)
            {
                _inBounds = true;
                _vcam.Follow = _player.transform;
            }
        }
    }
}
