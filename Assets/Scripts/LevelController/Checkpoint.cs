using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // enum for animation states
    private enum AnimState { asleep, idle }

    private AnimState _state = AnimState.asleep;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // change animation state from asleep to idle
    public void ActivateCheckpoint()
    {
        _state = AnimState.idle;
        _animator.SetInteger("state", (int)_state);
    }
}
