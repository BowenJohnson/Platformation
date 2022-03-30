using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// controls the main menu selections
public class MainMenuController : MonoBehaviour
{
    // reference to the canvas group, other animators, and audio source
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private MainMenuAnimControl _door;
    [SerializeField] private MainMenuAnimControl _camera;
    [SerializeField] private AudioClip _selectSFX;
    [SerializeField] private AudioClip _doorSFX;

    // reference to UI for after registration
    [SerializeField] private GameObject _survivalMenu;
    [SerializeField] private GameObject _registerMenu;
    [SerializeField] private GameObject _playerDataMenu;

    // field for error message clearing
    [SerializeField] private TMP_Text _registerError;

    private AudioSource _audio;

    // delay between scene transition
    private float _doorDelay = 1f;
    private float _zoomDelay = 2f;
    private float _sceneDelay = 3f;

    private void Awake()
    {
        // get the parent's audio source
        _audio = GetComponentInParent<AudioSource>();
    }

    public void PlaySurvival()
    {
        // deactivate UI
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        // give door time to open
        StartCoroutine(DelayDoor("Survival"));
    }

    public void PlayStory()
    {
        // deactivate UI
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        // give door time to open
        StartCoroutine(DelayDoor("Level_1"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // plays selection SFX called by button press
    public void PlaySelectSFX()
    {
        // play select SFX
        _audio.PlayOneShot(_selectSFX);
    }

    // called after successful registration
    // returns to survival login menu
    public void SurvivalLoginUI()
    {
        _survivalMenu.SetActive(true);
        _registerMenu.SetActive(false);
    }

    public void PlayerDataUI()
    {
        _survivalMenu.SetActive(false);
        _playerDataMenu.SetActive(true);
    }

    // gives UI time to dissapear before door opens
    IEnumerator DelayDoor(string scene)
    {
        yield return new WaitForSeconds(_doorDelay);

        // play door open SFX
        _audio.PlayOneShot(_doorSFX);

        // start open door animation
        _door.StartAnimation();

        StartCoroutine(DelayZoom(scene));
    }

    // reset error message
    public void ClearErrorMessage()
    {
        _registerError.text = null;
    }

    // waits for the door animation to play then zooms in
    IEnumerator DelayZoom(string scene)
    {
        yield return new WaitForSeconds(_zoomDelay);

        // zoom in camera
        _camera.StartAnimation();

        StartCoroutine(DelayTransition(scene));
    }

    // waits the delay then changes scenes
    IEnumerator DelayTransition(string scene)
    {
        yield return new WaitForSeconds(_sceneDelay);

        // start next scene
        SceneManager.LoadScene(scene);
    }
}
