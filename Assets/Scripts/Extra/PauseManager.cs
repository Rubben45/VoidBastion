using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseManager : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Image pauseBackground;
    
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button restartButton;


    private void Start()
    {
        InitializePauseMenu();
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        restartButton.onClick.AddListener(RestartButtonClicked);
    }

   private void InitializePauseMenu()
    {
        pauseMenu.SetActive(false);
        pauseBackground.color = Color.clear;
        
        continueButton.transform.localScale = Vector3.zero;
        homeButton.transform.localScale = Vector3.zero;
        restartButton.transform.localScale = Vector3.zero;
    }

    public void OnPauseButtonClicked()
    {
        Time.timeScale = 1f;  // Asigurăm că timeScale este normal înainte de pauză
        
        pauseMenu.SetActive(true);
        SetButtonsInteractable(false);

        pauseBackground.DOColor(new Color(0f, 0f, 0f, 0.55f), 0.2f).SetUpdate(true).OnComplete(() =>
        {
            continueButton.transform.DOScale(Vector3.one, 0.1f).SetUpdate(true);
            homeButton.transform.DOScale(Vector3.one, 0.2f).SetUpdate(true);
            restartButton.transform.DOScale(Vector3.one, 0.3f).SetUpdate(true).OnComplete(() =>
            {
                SetButtonsInteractable(true);
                Time.timeScale = 0f;  // Pauză efectivă după animație
            });
        });
    }

    public void OnContinueButtonClicked()
    {
        SetButtonsInteractable(false);
        Time.timeScale = 1f;  // Resetăm timeScale înainte de animații
        
        continueButton.transform.DOScale(Vector3.zero, 0.2f).SetUpdate(true);
        homeButton.transform.DOScale(Vector3.zero, 0.2f).SetUpdate(true);
        restartButton.transform.DOScale(Vector3.zero, 0.2f).SetUpdate(true).OnComplete(() =>
        {
            pauseBackground.DOColor(Color.clear, 0.2f).SetUpdate(true).OnComplete(() =>
            {
                pauseMenu.SetActive(false);
                pauseButton.interactable = true;
            });
        });
    }

    public void RestartButtonClicked()
    {
        levelLoader.OnPlayButtonClicked(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetButtonsInteractable(bool state)
    {
        continueButton.interactable = state;
        homeButton.interactable = state;
        restartButton.interactable = state;
        pauseButton.interactable = state;
    }
}
