using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using VInspector;

public class MainMenu : MonoBehaviour
{
    [Tab("MainMenu")]
    [SerializeField] private Button[] menuButtons;
    
    [SerializeField] private ConsistentNumber consistentNumber;
    [SerializeField] private Image transitionObject;
    
    [Tab("SettingsMenu")]
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private RectTransform settingsMenuBackground;
    [SerializeField] private GameObject realSettings;
    
    [Tab("ChooseGameModeMenu")]
    [SerializeField] private GameObject chooseGameModeMenu;

    [SerializeField] private Button tutorialModeButton;
    [SerializeField] private Button normalModeButton;
    private void Awake()
    {
        if (!consistentNumber.alreadyShown)
        {
            transitionObject.enabled = false;
            consistentNumber.alreadyShown = true;
        }
        else
        {
            transitionObject.enabled = true;
        }

        StartCoroutine(ShowTransitionObejct());
    }

    private void Start()
    {
        InitializeMenuButtons();
        StartCoroutine(ShowMenuButtons());
    }

    private void InitializeMenuButtons()
    {
        foreach (var button in menuButtons)
        {
            button.interactable = false;
            button.gameObject.transform.localScale = Vector3.zero;
        }
        
        tutorialModeButton.interactable = false;
        normalModeButton.interactable = false;
        
        settingsMenu.SetActive(false);
        settingsMenuBackground.gameObject.SetActive(false);
        settingsMenuBackground.sizeDelta = Vector2.zero;
        
        realSettings.SetActive(false);
        chooseGameModeMenu.SetActive(false);
    }
    

    private IEnumerator ShowMenuButtons()
    {
        foreach (var button in menuButtons)
        {
            button.interactable = false;
            button.gameObject.SetActive(true);
        }
        
        yield return new WaitForSeconds(0.25f);
        
        menuButtons[0].gameObject.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
        
        yield return new WaitForSeconds(0.25f);
        
        menuButtons[1].gameObject.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
        
        yield return new WaitForSeconds(0.25f);

        menuButtons[2].gameObject.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            foreach (var button in menuButtons)
            {
                button.interactable = true;
            }
        });
    }

    private IEnumerator ShowTransitionObejct()
    {
        yield return new WaitForSeconds(1f);
        
        transitionObject.enabled = true;
    }

    public void OnSettingsButtonClicked()
    {
        foreach (var button in menuButtons)
        {
            button.interactable = false;
            button.gameObject.transform.DOScale(0f, 0.1f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                 button.gameObject.SetActive(false);
                 settingsMenu.SetActive(true);
                 
                 settingsMenuBackground.gameObject.SetActive(true);
                 settingsMenuBackground.DOSizeDelta(new Vector2(625f, 450f), 0.5f).OnComplete(() =>
                 {
                      realSettings.SetActive(true);
                 });
            });
        }
    }

    public void OnQuitSettingsButtonClicked()
    {
        realSettings.SetActive(false);

        StartCoroutine(ShowMenuButtons());
        settingsMenuBackground.DOSizeDelta(Vector2.zero, 0.25f).OnComplete(() =>
        {
            settingsMenuBackground.gameObject.SetActive(false);
            settingsMenu.SetActive(false);
        });
    }

    public void OnPlayButtonClicked()
    {
        foreach (var button in menuButtons)
        {
            button.interactable = false;
            button.gameObject.transform.DOScale(0f, 0.1f).OnComplete(() =>
            {
                chooseGameModeMenu.SetActive(true);
                tutorialModeButton.gameObject.transform.DOScale(1f, 0.5f);
                normalModeButton.gameObject.transform.DOScale(1f, 0.5f).OnComplete(() =>
                {
                     normalModeButton.interactable = true;
                     tutorialModeButton.interactable = true;
                });
            });
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
