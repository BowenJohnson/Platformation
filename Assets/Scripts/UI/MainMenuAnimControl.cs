using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimControl : MonoBehaviour
{
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void StartAnimation()
    {
        _anim.SetTrigger("Start");
    }
}
