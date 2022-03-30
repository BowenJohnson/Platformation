using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //public static bool gamePaused = false;
    public static bool gamePaused { get; set; } = false;

    private float _delayTime = 1f;

    private string _mainMenuScene = "StartMenu";

    // pause UI reference
    [SerializeField] private GameObject _pauseUI;

    // level loader UI reference
    [SerializeField] private GameObject levelLoader;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // resumes time in game and deactivates pause menu on screen
    // sets game paused menu to false
    private void Resume()
    {
        _pauseUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    // activates pause menu on screen and freezes time in the game
    // sets game paused bool to true
    private void Pause()
    {
        _pauseUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    // UI resume button in case player clicks resume instead of escape
    public void ResumeButton()
    {
        Resume();
    }

    // UI exit button to quit to main menu
    public void ExitButton()
    {
        // start time back up
        Time.timeScale = 1f;

        // set paused to false to prevent player input bugs
        gamePaused = false;

        // call level loader to do fade out scene transition
        // pause for the animation then reload scene
        levelLoader.GetComponent<LevelLoader>().FadeOut();
        StartCoroutine(DelayExitTransition());
    }

    // delay for number of seconds to allow crossfade animation
    // to finish then reload scene to start over
    IEnumerator DelayExitTransition()
    {
        yield return new WaitForSeconds(_delayTime);
        SceneManager.LoadScene(_mainMenuScene);
    }
}
