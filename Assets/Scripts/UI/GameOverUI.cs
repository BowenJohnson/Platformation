using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    // UI text
    [SerializeField] private TextMeshProUGUI _tileCount;
    [SerializeField] private TextMeshProUGUI _treasureCount;
    [SerializeField] private TextMeshProUGUI _killCount;

    // level loader UI reference
    [SerializeField] private GameObject levelLoader;

    private float _fadeDuration = 0.4f;
    private float _delayTime = 1f;
    private float _gameOverDelay = 2f;

    private string _mainMenuScene = "StartMenu";
    private string _survivalScene = "Survival";

    // game over flag for activating restart
    public bool gameOver { get; set; }

    private void Awake()
    {
        DisableUI();
    }

    void Start()
    {
        // set everything to 0
        _tileCount.text = 0.ToString();
        _treasureCount.text = 0.ToString();
        _killCount.text = 0.ToString();
    }

    // set all vars to passed in totals
    public void SetValues(int tiles, int treasure, int kills)
    {
        _tileCount.text = tiles.ToString();
        _treasureCount.text = treasure.ToString();
        _killCount.text = kills.ToString();
    }

    // activate the UI and display it on screen
    // and wait for player input to restart after delay
    public void EnableUI()
    {
        StartCoroutine(DelayGameOver());
    }

    // deactivate the UI and hide it on screen
    public void DisableUI()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    // UI restart button function to start the level over
    public void RestartButton()
    {
        // call level loader to do fade out scene transition
        // pause for the animation then reload scene
        levelLoader.GetComponent<LevelLoader>().FadeOut();
        StartCoroutine(DelayRestartTransition());
    }

    // UI exit button to quit to main menu
    public void ExitButton()
    {
        // log user out of game
        FindObjectOfType<FirebaseController>().SignOut();

        // call level loader to do fade out scene transition
        // pause for the animation then reload scene
        levelLoader.GetComponent<LevelLoader>().FadeOut();
        StartCoroutine(DelayExitTransition());
    }   

    // fade in UI over shor duration
    private IEnumerator FadeIn(CanvasGroup canvasGroup, float start, float end)
    {
        float counter = 0f;

        while (counter < _fadeDuration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / _fadeDuration);

            yield return null;
        }
    }

    // delay for number of seconds to allow crossfade animation
    // to finish then reload scene to start over
    IEnumerator DelayRestartTransition()
    {
        yield return new WaitForSeconds(_delayTime);
        SceneManager.LoadScene(_survivalScene);
    }

    // delay for number of seconds to allow crossfade animation
    // to finish then reload scene to start over
    IEnumerator DelayExitTransition()
    {
        yield return new WaitForSeconds(_delayTime);
        SceneManager.LoadScene(_mainMenuScene);
    }

    // delays the fade in of the game over UI 
    IEnumerator DelayGameOver()
    {
        yield return new WaitForSeconds(_gameOverDelay);

        StartCoroutine(FadeIn(_canvasGroup, _canvasGroup.alpha, 1f));
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        // set game ove to true and start restart check loop
        gameOver = true;
    }
}
