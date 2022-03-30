using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryEndController : MonoBehaviour
{
    // level loader UI, fade out delay, and exit to scene
    private GameObject _levelLoader;
    private float _delayTime = 1f;
    private string _exitToScene = "StartMenu";

    [Header("Sound Effects")]
    private BasicMobSFX _sfx;
    [SerializeField] private AudioClip _wizardLaugh;

    // Start is called before the first frame update
    void Start()
    {
        _sfx = GetComponent<BasicMobSFX>();

        // put this in a try catch so it doesn't blow up while testing with level loader off
        try
        {
            _levelLoader = FindObjectOfType<LevelLoader>().gameObject;
        }
        catch (Exception ex)
        {
            Debug.Log("Exception: " + ex);
        }
    }

    // check if player hits escape or return to exit to title
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
        {
            ExitToTitle();
        }
    }

    // called by the animator to play wizard laugh
    public void PlayLaugh()
    {
        _sfx.PlaySound(_wizardLaugh);
    }

    // called by animator to exit to start screen
    public void ExitToTitle()
    {
        StartCoroutine(DelayExitTransition());
    }

    // delay for number of seconds to allow crossfade animation
    // to finish then reload scene to start over
    IEnumerator DelayExitTransition()
    {
        _levelLoader.GetComponent<LevelLoader>().FadeOut();
        yield return new WaitForSeconds(_delayTime);
        SceneManager.LoadScene(_exitToScene);
    }
}
