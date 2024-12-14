using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class UpgradesManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradeZone;
    [SerializeField] private GameObject buyParticles;

    [SerializeField] private Image damageUpgradeIcon;
    [SerializeField] private Image rangeUpgradeIcon;
    [SerializeField] private Image attackSpeedUpgradeIcon;
    [SerializeField] private Image healthUpgradeIcon;
    
    [SerializeField] private TextMeshProUGUI upgradeAmountText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private Sprite normalUpgradeSprite;
    [SerializeField] private Sprite maxUpgradeSprite;
    
    [SerializeField] private EconomyManager economyManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerStats playerStats;
    
    private PlayerData playerData;
    
    private readonly float[] damageUpgrades = { 10, 15, 20, 25, 30 };
    private readonly float[] rangeUpgrades = { 3.75f, 4f, 4.25f, 4.5f, 5f };
    private readonly float[] attackSpeedUpgrades = { 1.5f, 1.25f, 1f, 0.75f, 0.5f };
    private readonly float[] healthUpgrades = { 100, 125, 150, 175, 200 };

    private float currentUpgradePrice;

    private bool CanAfford()
    {
        return currentUpgradePrice <= economyManager.CurrentCurrency;
    }
    private bool IsMaxLevel(StatType statType)
    {
        return statType switch
        {
            StatType.Damage => playerData.Damage >= damageUpgrades[^1],
            StatType.Range => playerData.Range >= rangeUpgrades[^1],
            StatType.AttackSpeed => playerData.Speed <= attackSpeedUpgrades[^1],
            StatType.Health => playerData.Health >= healthUpgrades[^1],
            _ => false
        };
    }
    
    private bool AreAllStatsMaxLevel()
    {

        var damageMax = playerData.Damage >= damageUpgrades[^1];
        var rangeMax = playerData.Range >= rangeUpgrades[^1];
        var attackSpeedMax = playerData.Speed <= attackSpeedUpgrades[^1];
        var healthMax = playerData.Health >= healthUpgrades[^1];


        return damageMax && rangeMax && attackSpeedMax && healthMax;
    }
    private void Awake()
    {
        playerData = new PlayerData();
        
        playerController.SetPlayerData(playerData);
        playerStats.SetPlayerData(playerData);
    }

    private void Start()
    {
        currentUpgradePrice = 25f;

        UpdateUI();
    }
    

    private void UpgradeStatistic(StatType statType)
    {
        if (!CanAfford() && !IsMaxLevel(statType))
        {
            ShowInsufficientCurrencyWarning();
            return;
        }
        
        switch (statType)
        {
            case StatType.Damage:
                if (playerData.Damage < damageUpgrades[^1]) UpgradeDamage();
                else ShowMaxLevelIcon(damageUpgradeIcon);
                break;

            case StatType.Range:
                if (playerData.Range < rangeUpgrades[^1]) UpgradeRange();
                else ShowMaxLevelIcon(rangeUpgradeIcon);
                break;

            case StatType.AttackSpeed:
                if (playerData.Speed > attackSpeedUpgrades[^1]) UpgradeAttackSpeed();
                else ShowMaxLevelIcon(attackSpeedUpgradeIcon);
                break;

            case StatType.Health:
                if (playerData.Health < healthUpgrades[^1]) UpgradeHealth();
                else ShowMaxLevelIcon(healthUpgradeIcon);
                break;
        }

        if (!IsMaxLevel(statType)) DeductCurrencyAndIncreasePrice();
        
        UpdateUI();
    }
    
    private void DeductCurrencyAndIncreasePrice()
    {
        economyManager.DeductCurrency(currentUpgradePrice);

        currentUpgradePrice += 40f; 

        UpdateUI();
    }
    
    private int GetNextLevel(float currentValue, float[] upgradeArray)
    {
        for (var i = 0; i < upgradeArray.Length; i++)
        {
            if (currentValue < upgradeArray[i])
            {
                return i;
            }
        }
        return upgradeArray.Length - 1; 
    }
    
    private int GetNextLevelForAttackSpeed(float currentValue, float[] upgradeArray)
    {
        for (var i = 0; i < upgradeArray.Length; i++)
        {
            if (currentValue > upgradeArray[i]) 
            {
                return i;
            }
        }
        return upgradeArray.Length - 1; 
    }

    private void UpdateUI()
    {
        upgradeZone.SetActive(!AreAllStatsMaxLevel());

        upgradeAmountText.text = currentUpgradePrice.ToString("N0");

        damageText.text = playerData.Damage.ToString("N0"); 
        rangeText.text = playerData.Range.ToString();
        attackSpeedText.text = playerData.Speed.ToString();
        healthText.text = playerData.Health.ToString("N0");
        
        CheckTextPosition();
        
        playerController.CheckForStatsUpdate();
        playerStats.CheckForStatsUpdate();
    }

    private void ShowInsufficientCurrencyWarning()
    {
        upgradeAmountText.DOColor(Color.red, 0.15f).OnComplete(() =>
        {
            upgradeAmountText.DOColor(Color.white, 0.15f);
        });
    }
    

    private void CheckTextPosition()
    {
        switch (currentUpgradePrice)
        {
            case < 100:
                upgradeZone.transform.DOLocalMoveX(-20f, 0.25f);
                break;
            case >= 100:
                upgradeZone.transform.DOLocalMoveX(-8f, 0.25f);
                break;
        }
    }
    
    private void ShowMaxLevelIcon(Image upgradeIcon)
    {
        upgradeIcon.sprite = maxUpgradeSprite;
        
        if (DOTween.IsTweening(upgradeIcon.transform)) return;
        
        upgradeIcon.transform.DOLocalMoveY(-360f, 0.5f).OnComplete(() =>
        {
            upgradeIcon.transform.DOLocalMoveY(-420f, 0.5f);
        });

    }

    private void UpgradeDamage()
    {
        var nextLevel = GetNextLevel(playerData.Damage, damageUpgrades);
        playerData.UpgradeDamage(damageUpgrades[nextLevel]);


        if (DOTween.IsTweening(damageUpgradeIcon.transform)) return;
        
        damageUpgradeIcon.transform.DOLocalMoveY(-360f, 0.5f).OnComplete(() =>
        {
              damageUpgradeIcon.transform.DOLocalMoveY(-420f, 0.5f);
        });
    }

    private void UpgradeRange()
    {
        var nextLevel = GetNextLevel(playerData.Range, rangeUpgrades);
        playerData.UpgradeRange(rangeUpgrades[nextLevel]);
        
        
        if (DOTween.IsTweening(rangeUpgradeIcon.transform)) return;
        
        rangeUpgradeIcon.transform.DOLocalMoveY(-360f, 0.5f).OnComplete(() =>
        {
            rangeUpgradeIcon.transform.DOLocalMoveY(-420f, 0.5f);
        });
    }

    private void UpgradeAttackSpeed()
    {
        var nextLevel = GetNextLevelForAttackSpeed(playerData.Speed, attackSpeedUpgrades);
        playerData.UpgradeSpeed(attackSpeedUpgrades[nextLevel]);

        
        if (DOTween.IsTweening(attackSpeedUpgradeIcon.transform)) return;
        
        attackSpeedUpgradeIcon.transform.DOLocalMoveY(-360f, 0.5f).OnComplete(() =>
        {
            attackSpeedUpgradeIcon.transform.DOLocalMoveY(-420f, 0.5f);
        });
    }

    private void UpgradeHealth()
    {
        var nextLevel = GetNextLevel(playerData.Health, healthUpgrades);
        playerData.UpgradeHealth(healthUpgrades[nextLevel]);


        if (DOTween.IsTweening(healthUpgradeIcon.transform)) return;
            
        healthUpgradeIcon.transform.DOLocalMoveY(-360f, 0.5f).OnComplete(() =>
        {
            healthUpgradeIcon.transform.DOLocalMoveY(-420f, 0.5f);
        });
    }

    public void RemoteUpdateUI()
    {
        UpdateUI();
    }

    #region ---- ButtonsSettings ----
    
    public void OnUpgradeDamage()
    {
        UpgradeStatistic(StatType.Damage);
    }

    public void OnUpgradeRange()
    {
        UpgradeStatistic(StatType.Range);
    }

    public void OnUpgradeAttackSpeed()
    {
        UpgradeStatistic(StatType.AttackSpeed);
    }

    public void OnUpgradeHealth()
    {
        UpgradeStatistic(StatType.Health);
    }
    
    #endregion
}

public enum StatType
{
    Damage,
    Range,
    AttackSpeed,
    Health
}
