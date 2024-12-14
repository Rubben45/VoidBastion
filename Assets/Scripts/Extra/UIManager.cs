using TMPro;
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using VInspector;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Tab("UI Elements")]
    [SerializeField] private AbilityManager abilityManager;
    
    [SerializeField] private Slider healthSlider_right;
    [SerializeField] private Slider experienceSlider_right;
    
    [SerializeField] private Slider healthSlider_left;
    [SerializeField] private Slider experienceSlider_left;
    
    [SerializeField] private Image currentLevelIconSprite;
    [SerializeField] private Sprite[] allLevelSprites;

    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private Image currentLevelBG_1_Right;
    [SerializeField] private Image currentLevelBG_2_Right;
    [SerializeField] private Image currentLevelBG_1_Left;
    [SerializeField] private Image currentLevelBG_2_Left;
    
    [Tab("Abilities Elements")]
    [SerializeField] private RectTransform abilityCard_Left;
    [SerializeField] private RectTransform abilityCard_Right;

    [SerializeField] private GameObject abilityCardInfoHolder_Left;
    [SerializeField] private GameObject abilityCardInfoHolder_Right;
    
    [SerializeField] private Image abilityIcon_Left;
    [SerializeField] private Image abilityIcon_Right;
    
    [SerializeField] private TextMeshProUGUI abilityName_Left;
    [SerializeField] private TextMeshProUGUI abilityName_Right;
    
    [SerializeField] private TextMeshProUGUI abilityDescription_Left;
    [SerializeField] private TextMeshProUGUI abilityDescription_Right;
    
    private SpecialAbility abilityLeft;
    private SpecialAbility abilityRight;
    
    [Tab("Abilities Icons")]
    [SerializeField] private GameObject blastHit_Icon;
    [SerializeField] private GameObject pierceHit_Icon;
    [SerializeField] private GameObject freezeHit_Icon;
    [SerializeField] private GameObject magneticField_Icon;
    [SerializeField] private GameObject satellite_Icon;
    
    
    private bool hasAnimatedOnce = false;
    
    private void Start()
    {
        InitializeAbilityCards();
    }
    
    private void InitializeAbilityCards()
    {
        abilityCard_Left.gameObject.SetActive(false);
        abilityCard_Right.gameObject.SetActive(false);

        abilityCard_Left.sizeDelta = new Vector2(abilityCard_Left.sizeDelta.x, 0f);
        abilityCard_Right.sizeDelta = new Vector2(abilityCard_Right.sizeDelta.x, 0f);

        abilityCardInfoHolder_Left.SetActive(false);
        abilityCardInfoHolder_Right.SetActive(false);
        
        blastHit_Icon.SetActive(false);
        pierceHit_Icon.SetActive(false);
        freezeHit_Icon.SetActive(false);
        magneticField_Icon.SetActive(false);
        satellite_Icon.SetActive(false);
        
        Time.timeScale = 1f;
    }

    private void UpdatePlayerUI(float valueToChange, PlayerStatsType playerStatsType)
    {
        switch (playerStatsType)
        {
            case PlayerStatsType.Health: UpdatePlayerHealth(valueToChange);
                break;
            
            case PlayerStatsType.Experience: UpdatePlayerExperience(valueToChange);
                break;
            
            default: throw new NotImplementedException();
        }
    }

    private void UpdatePlayerHealth(float valueChanged)
    {
        healthSlider_right.DOValue(valueChanged, 0.25f);
        healthSlider_left.DOValue(valueChanged, 0.25f);
    }

    private void UpdatePlayerExperience(float valueToChange)
    {
        experienceSlider_right.DOValue(valueToChange, 0.25f);
        experienceSlider_left.DOValue(valueToChange, 0.25f);
    }

    private void UpdateCurrentLevelIcon(int currentLevel)
    {
        currentLevelIconSprite.sprite = currentLevel switch
        {
            1 => allLevelSprites[0],
            2 => allLevelSprites[0],
            3 => allLevelSprites[1],
            4 => allLevelSprites[1],
            5 => allLevelSprites[2],
            6 => allLevelSprites[2],
            7 => allLevelSprites[3],
            8 => allLevelSprites[3],
            9 => allLevelSprites[4],
            10 => allLevelSprites[4],
            _ => throw new NotImplementedException()
        };

        currentLevelBG_1_Right.sprite = currentLevelBG_2_Right.sprite = currentLevelBG_1_Left.sprite = currentLevelBG_2_Left.sprite = currentLevelIconSprite.sprite;
        currentLevelText.text = currentLevel.ToString();
    }

    public void ShowAvailableAbilitiesForSelection(List<SpecialAbility> selectedAbilities)
    {
        if (selectedAbilities.Count < 2) return;
        
            // Setăm prima abilitate pe cardul din stânga
            abilityLeft = selectedAbilities[0];
            abilityIcon_Left.sprite = abilityLeft.AbilityIcon;
            abilityName_Left.text = abilityLeft.AbilityName;
            abilityDescription_Left.text = abilityLeft.AbilityDescription;

            // Setăm a doua abilitate pe cardul din dreapta
            abilityRight = selectedAbilities[1];
            abilityIcon_Right.sprite = abilityRight.AbilityIcon;
            abilityName_Right.text = abilityRight.AbilityName;
            abilityDescription_Right.text = abilityRight.AbilityDescription;
            
            AnimateAbilityCards();
    }
    
    private void AnimateAbilityCards()
    {
        if (!hasAnimatedOnce)
        {
            hasAnimatedOnce = true;
            return;
        }
        
        abilityCard_Left.gameObject.SetActive(true);
        abilityCard_Right.gameObject.SetActive(true);
        
        abilityCardInfoHolder_Left.SetActive(false);
        abilityCardInfoHolder_Right.SetActive(false);
        
        abilityCard_Left.sizeDelta = new Vector2(abilityCard_Left.sizeDelta.x, 0f);
        abilityCard_Right.sizeDelta = new Vector2(abilityCard_Right.sizeDelta.x, 0f);
        
        abilityCard_Left.DOSizeDelta(new Vector2(abilityCard_Left.sizeDelta.x, 650f), 0.5f).OnComplete(() =>
        {
            abilityCardInfoHolder_Left.SetActive(true);
        });
        
        abilityCard_Right.DOSizeDelta(new Vector2(abilityCard_Right.sizeDelta.x, 650f), 0.5f).OnComplete(() =>
        {
            abilityCardInfoHolder_Right.SetActive(true);
            Time.timeScale = 0f;
        });
    }

    private void HideAbilityCards()
    {
        abilityCardInfoHolder_Left.SetActive(false);
        abilityCardInfoHolder_Right.SetActive(false);
        
        abilityCard_Left.DOSizeDelta(new Vector2(abilityCard_Left.sizeDelta.x, 0f), 0.5f).OnComplete(() =>
        {
            abilityCard_Left.gameObject.SetActive(false);
        });
        
        abilityCard_Right.DOSizeDelta(new Vector2(abilityCard_Right.sizeDelta.x, 0f), 0.5f).OnComplete(() =>
        {
            abilityCard_Left.gameObject.SetActive(false);
        });
    }

    private void ShowAbilityIcon(SpecialAbilityType abilityType)
    {
        switch (abilityType)
        {
            case SpecialAbilityType.BlastShot: if (!blastHit_Icon.activeSelf) blastHit_Icon.SetActive(true); break;
            case SpecialAbilityType.FreezeShot: if (!freezeHit_Icon.activeSelf) freezeHit_Icon.SetActive(true); break;
            case SpecialAbilityType.MagneticField: if (!magneticField_Icon.activeSelf) magneticField_Icon.SetActive(true); break;
            case SpecialAbilityType.PierceShot: if (!pierceHit_Icon.activeSelf) pierceHit_Icon.SetActive(true); break;
            case SpecialAbilityType.Satellite: if (!satellite_Icon.activeSelf) satellite_Icon.SetActive(true); break;
            default: Debug.LogError("Unknown Special Ability Type"); break;
        }
    }

    public void OnAbilityLeftClicked()
    {
        Time.timeScale = 1f;
        
        HideAbilityCards();
        
        abilityManager.SelectAbility(abilityLeft);
    }

    public void OnAbilityRightClicked()
    {
        Time.timeScale = 1f;
        
        HideAbilityCards();
        
        abilityManager.SelectAbility(abilityRight);
    }

    private void OnEnable()
    {
        PlayerStats.OnPlayerStatsChanged += UpdatePlayerUI;
        PlayerStats.OnLevelChanged += UpdateCurrentLevelIcon;
        PlayerAbilityInventory.OnSpecialAbilityChanged += ShowAbilityIcon;
    }

    private void OnDisable()
    {
        PlayerStats.OnPlayerStatsChanged -= UpdatePlayerUI;
        PlayerStats.OnLevelChanged -= UpdateCurrentLevelIcon;
        PlayerAbilityInventory.OnSpecialAbilityChanged -= ShowAbilityIcon;
    }
}
