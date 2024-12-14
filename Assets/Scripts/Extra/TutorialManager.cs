using System.Collections;
using DG.Tweening;
using Enemies;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private SpriteRenderer backgroundRenderer;
    
    [SerializeField] private GameObject[] masksObjects;
    [SerializeField] private GameObject[] tutorialTexts;

    [SerializeField] private Button[] upgradesButtons;
    [SerializeField] private Button playerStatusButton;
    [SerializeField] private Button continueButton_1;
    [SerializeField] private Button exitStatusButton_1;

    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private Transform spawnPoint;
    
    [SerializeField] private MenusManager menusManager;
    [SerializeField] private LevelLoader levelLoader;
    

    private void Start()
    {
        Initialize();
        
        continueButton_1.onClick.AddListener(OnContinueButtonClicked_1);
        playerStatusButton.onClick.AddListener(OnContinueButtonClicked_2);
        upgradesButtons[0].onClick.AddListener(OnContinueButtonClicked_3);
        
        ShowFirstStep();
    }

    private void Initialize()
    {
        foreach (var mask in masksObjects)
        {
            mask.transform.localScale = Vector3.zero;
        }

        foreach (var tutorialText in tutorialTexts)
        {
            tutorialText.transform.localScale = Vector3.zero;
        }

        foreach (var upgradeBTN in upgradesButtons)
        {
            upgradeBTN.interactable = false;
        }
        
        continueButton_1.transform.localScale = Vector3.zero;
    }

    private void ShowFirstStep()
    {
        playerStatusButton.interactable = false;
        
        masksObjects[0].transform.DOScale(GetMaskScale(0), .5f);
        tutorialTexts[0].transform.DOScale(1f, .5f).OnComplete(() =>
        {
            masksObjects[1].transform.DOScale(GetMaskScale(1), .5f);
            tutorialTexts[1].transform.DOScale(1f, .5f).OnComplete(() =>
            {
                masksObjects[2].transform.DOScale(GetMaskScale(2), .5f);
                tutorialTexts[2].transform.DOScale(1f, .5f).OnComplete(() =>
                {
                    continueButton_1.transform.DOScale(1f, 0.5f).OnComplete(() =>
                    {
                        continueButton_1.interactable = true;
                    });
                });
            });
        });
    }
    
    private void OnContinueButtonClicked_1()
    {
        continueButton_1.interactable = false;
        continueButton_1.transform.DOScale(0f, 0.25f);
        continueButton_1.onClick.RemoveAllListeners();
        
        
        masksObjects[0].transform.DOScale(0f, .5f);
        tutorialTexts[0].transform.DOScale(0f, .5f).OnComplete(() =>
        {
            masksObjects[1].transform.DOScale(0f, .5f);
            tutorialTexts[1].transform.DOScale(0f, .5f).OnComplete(() =>
            {
                masksObjects[2].transform.DOScale(0f, .5f);
                tutorialTexts[2].transform.DOScale(0f, .5f).OnComplete(() =>
                {
                    masksObjects[3].transform.DOScale(GetMaskScale(3), .5f);
                    tutorialTexts[3].transform.DOScale(1f, 0.5f).OnComplete(() =>
                    {
                        playerStatusButton.interactable = true;
                        exitStatusButton_1.enabled = false;
                    });
                });
            });
        });
    }

    private void OnContinueButtonClicked_2()
    {
        playerStatusButton.onClick.RemoveAllListeners();
        playerStatusButton.interactable = false;
        
        masksObjects[3].transform.DOScale(0f, .5f);
        tutorialTexts[3].transform.DOScale(0f, .5f).OnComplete(() =>
        {
             masksObjects[4].transform.DOScale(GetMaskScale(4), .5f);
             tutorialTexts[4].transform.DOScale(1f, 0.5f).OnComplete(() =>
             {
                 StartCoroutine(ShowEnemyPart());
             });
        });
    }

    private IEnumerator ShowEnemyPart()
    {
        SpawnEnemy();
        
        yield return new WaitForSeconds(3f);
        
        masksObjects[4].transform.DOScale(0f, .5f);
        tutorialTexts[4].transform.DOScale(0f, .5f);
        
        yield return new WaitForSeconds(2f);
        masksObjects[5].transform.DOScale(GetMaskScale(5), .5f);
        tutorialTexts[5].transform.DOScale(1f, .5f);

        yield return new WaitForSeconds(2f);
        
        SpawnEnemy();
        tutorialTexts[5].transform.DOScale(0f, .5f);
        masksObjects[5].transform.DOScale(50f, 0.5f).OnComplete(() =>
        {
            backgroundRenderer.color = Color.clear;
            backgroundRenderer.gameObject.SetActive(false);
            masksObjects[5].gameObject.SetActive(false);
        });
        
        yield return new WaitForSeconds(17f);
        ShowSecondStep();
    }

    private void ShowSecondStep()
    {
        backgroundRenderer.gameObject.SetActive(true);
        backgroundRenderer.DOColor(new Color(0f, 0f, 0f, 0.65f), 0.5f).OnComplete(() =>
        {
            masksObjects[6].transform.DOScale(GetMaskScale(6), .5f);
            tutorialTexts[6].transform.DOScale(1f, 0.5f).OnComplete(() =>
            {
                upgradesButtons[0].interactable = true;
            });
        });
    }

    private void OnContinueButtonClicked_3()
    {
        StartCoroutine(ShowSecondEnemyPart());
        tutorialTexts[6].transform.DOScale(0f, .5f).OnComplete(() =>
        {
             tutorialTexts[7].transform.DOScale(1f, .5f);
        });
    }

    private IEnumerator ShowSecondEnemyPart()
    {
        SpawnEnemy();
        
        yield return new WaitForSeconds(3f);
        
        menusManager.CloseStatsMenu();
        tutorialTexts[7].transform.DOScale(0f, .5f);
        backgroundRenderer.DOColor(Color.clear, 0.5f);
        playerStatusButton.interactable = false;
        
        yield return new WaitForSeconds(3f);
        
        SpawnEnemy();

        yield return new WaitForSeconds(15f);

        tutorialTexts[8].transform.DOScale(1f, 0.5f);
    }

    public void OnTutorialAbilitySelected()
    {
        tutorialTexts[8].transform.DOScale(0f, 0.5f).OnComplete(() =>
        {
            tutorialTexts[9].transform.DOScale(1f, 0.5f);
            StartCoroutine(GoHome());
        });
    }

    private IEnumerator GoHome()
    {
        yield return new WaitForSeconds(5f);
        
        levelLoader.OnPlayButtonClicked(0);
    }
    

    private void SpawnEnemy()
    {
        var instantiatedEnemy = Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
        instantiatedEnemy.GetComponent<Enemy>().target = player;
        
        print("Spawning enemy!");
    }

    private Vector3 GetMaskScale(int index)
    {
        // Define the scale for each mask object here, based on index
        return index switch
        {
            0 => new Vector3(2f, 2f, 1f),
            1 => new Vector3(3f, 3f, 1f),
            2 => new Vector3(8f, 1f, 1f),
            3 => new Vector3(1.5f, 1.5f, 1f),
            4 => new Vector3(8f, 2f, 1f),
            5 => new Vector3(2.5f, 2.5f, 1f),
            6 => new Vector3(1.5f, 1.5f, 1f),
            _ => Vector3.one
        };
    }
}
