using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimControl : MonoBehaviour
{
    [SerializeField] private GameObject _titleText;
    [SerializeField] private GameObject _menuText;
    [SerializeField] private CanvasGroup _menuCanvas;
    Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // called by animator to get rid of the text
    public void EnableMenu()
    {
        _menuText.SetActive(true);
    }

    // called by animator to get rid of the text
    public void DisableTitle()
    {
        _titleText.SetActive(false);
    }

    public void ActivateButtons()
    {
        _menuCanvas.interactable = true;
        _menuCanvas.blocksRaycasts = true;
    }

    public void ActivateUI()
    {
        _menuCanvas.alpha = 1;
        _menuCanvas.interactable = true;
        _menuCanvas.blocksRaycasts = true;
    }

    // needed in animator to make buttons go away
    public void DeactivateUI()
    {
        _menuCanvas.alpha = 0;
        _menuCanvas.interactable = false;
        _menuCanvas.blocksRaycasts = false;
    }

    public void ChangeAnimState()
    {
        _anim.SetBool("Deactive", true);
    }
}
