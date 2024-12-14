using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    [SerializeField] private Slider experienceSlider;

    [SerializeField] private Color damagedColor;
    
    [SerializeField] private float regenerationRate = 2f;

    private readonly float[] experienceNeededPerLevel = new float[10];

    private SpriteRenderer playerSprite;
    private PlayerData playerData;

    public static event Action<float, PlayerStatsType> OnPlayerStatsChanged;
    public static event Action<int> OnLevelChanged;

    private float maxHealth;
    private float currentHealth;
    
    private float currentExperience;

    private int currentLevel;

    private float regenPenalty = 7f;

    private void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        currentLevel = 1;
        
        InitializeXPPerLevel();
        InitializeUI();
    }
    
    public void SetPlayerData(PlayerData data)
    {
        playerData = data;

        maxHealth = playerData.Health;
    }

    public void CheckForStatsUpdate()
    {
        maxHealth = playerData.Health;
    }

    private void Update()
    {
        CheckForRegen();

        if (regenPenalty <= 0) return;
        
        regenPenalty -= Time.deltaTime;
    }

    private void InitializeXPPerLevel()
    {
        experienceNeededPerLevel[0] = 100f;
        experienceNeededPerLevel[1] = 200f;
        experienceNeededPerLevel[2] = 350f;
        experienceNeededPerLevel[3] = 500f;
        experienceNeededPerLevel[4] = 700f;
        experienceNeededPerLevel[5] = 1000f;
        experienceNeededPerLevel[6] = 1000f;
        experienceNeededPerLevel[7] = 1250f;
        experienceNeededPerLevel[8] = 1250f;
        experienceNeededPerLevel[9] = 1500f;
    }

    private void InitializeUI()
    {
        OnPlayerStatsChanged?.Invoke(currentHealth, PlayerStatsType.Health);
        OnPlayerStatsChanged?.Invoke(currentExperience, PlayerStatsType.Experience);
        
        OnLevelChanged?.Invoke(currentLevel);
    }

    public void GetInjured(float damage)
    {
        currentHealth -= damage;
        regenPenalty = 7f;
        
        SFXManager.Instance.GetDamageSound();
        
        var currentHealthPercent = currentHealth / maxHealth;
        OnPlayerStatsChanged?.Invoke(currentHealthPercent, PlayerStatsType.Health);

        if (DOTween.IsTweening(transform)) return;

        playerSprite.DOColor(damagedColor, 0.25f);
        transform.DOScale(0.8f, 0.25f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            playerSprite.DOColor(Color.white, 0.25f);
            transform.DOScale(1, 0.25f).SetEase(Ease.OutBounce);
        });
    }

    private void CheckForRegen()
    {
        if (currentHealth >= maxHealth) return;

        if (regenPenalty <= 0)
        {
            currentHealth += regenerationRate * Time.deltaTime;
            
            var currentHealthPercent = currentHealth / maxHealth;
            OnPlayerStatsChanged?.Invoke(currentHealthPercent, PlayerStatsType.Health);
        }
    }
    public void GainExperience(float experience)
    {
        if (currentLevel >= 10) return;

        currentExperience += experience;
        
        var currentExperiencePercent = currentExperience / experienceNeededPerLevel[currentLevel - 1];
        OnPlayerStatsChanged?.Invoke(currentExperiencePercent, PlayerStatsType.Experience);

        if (currentExperience >= experienceNeededPerLevel[currentLevel - 1]) LevelUp();
    }

    private void LevelUp()
    {
        currentExperience = 0f;
        currentLevel++;

        if (currentLevel <= 10) OnPlayerStatsChanged?.Invoke(currentExperience, PlayerStatsType.Experience);
        
        OnLevelChanged?.Invoke(currentLevel);
    }

}

public enum PlayerStatsType
{
    Health,
    Experience,
}
