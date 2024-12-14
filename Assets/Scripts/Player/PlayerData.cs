using UnityEngine;

public class PlayerData
{
    public float Damage { get; private set; }
    public float Range { get; private set; }
    public float Speed { get; private set; }
    public float Health { get; private set; }

    // Valorile inițiale
    private const float initialDamage = 10f;
    private const float initialRange = 3.75f;
    private const float initialSpeed = 1.5f;
    private const float initialHealth = 100f;

    public PlayerData()
    {
        ResetStats();
    }
    
    private void ResetStats()
    {
        Damage = initialDamage;
        Range = initialRange;
        Speed = initialSpeed;
        Health = initialHealth;
    }
    
    public void UpgradeDamage(float amount)
    {
        Damage = amount;
    }

    public void UpgradeRange(float amount)
    {
        Range = amount;
    }

    public void UpgradeSpeed(float amount)
    {
        Speed = amount;
    }

    public void UpgradeHealth(float amount)
    {
        Health = amount;
    }
}
