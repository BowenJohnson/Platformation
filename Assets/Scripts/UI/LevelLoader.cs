using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public bool fadeIn { get; set; }
    public bool fadeOut { get; set; }

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    public void FadeOut()
    {
        _anim.SetTrigger("Start");
    }
}
