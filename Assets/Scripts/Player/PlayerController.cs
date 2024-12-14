using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enemies;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject blastBullet;
    
    [SerializeField] private GameObject satellitePrefab;

    [SerializeField] private CircleCollider2D rangeCollider2D;
    
    [SerializeField] private EconomyManager economyManager;


    private PlayerStats playerStats;
    private PlayerAbilityInventory playerInventory;
    private  float reloadTime;
    private float reloadTimer;
    private readonly List<Enemy> enemiesInRange = new();

    private Transform currentTarget;
    private PlayerData playerData;
    
    private GameObject currentSatellite;
    
    private float aimRange;
    private float currentDamage;
    
    private int shotsFiredCounter;
    private const int shotsUntilBlastShot = 5;
    

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerInventory = GetComponent<PlayerAbilityInventory>();
        CheckForSatelliteAbility();
    }
    
    private void CheckForSatelliteAbility()
    {
        if (playerInventory.HasSatellite() && currentSatellite == null)
        {
            SpawnSatellite();
        }
    }
    
    private void SpawnSatellite()
    {
        if (currentSatellite != null) return;
        
        currentSatellite = Instantiate(satellitePrefab, transform.position, Quaternion.identity);
        var satelliteController = currentSatellite.GetComponent<Satellite>();
        satelliteController.Initialize(transform);
        
        var satelliteLevel = playerInventory.GetCurrentLevel(SpecialAbilityType.Satellite);
        satelliteController.SetSatelliteParameters(satelliteLevel);
        
        
    }
    
    public void SetPlayerData(PlayerData data)
    {
        playerData = data;

        reloadTime = playerData.Speed;
        aimRange = playerData.Range;
        currentDamage = playerData.Damage;
        
        rangeCollider2D.radius = aimRange;
    }

    public void CheckForStatsUpdate()
    {
        reloadTime = playerData.Speed;
        aimRange = playerData.Range;
        currentDamage = playerData.Damage;
        
        rangeCollider2D.radius = aimRange;
    }

    private void Update()
    {
        if (enemiesInRange.Count <= 0) return;
        
        UpdateTarget();
        RotateToTarget();

        reloadTimer += Time.deltaTime;
        
        if (reloadTimer < reloadTime) return;
        
        ShootEnemy();
        reloadTimer = 0f;
    }

    private void UpdateTarget()
    {
        var shortestDistance = Mathf.Infinity;
        Enemy nearestEnemy = null;
            
        foreach (var enemy in enemiesInRange)
        {
            var distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy >= shortestDistance) continue;
            
            shortestDistance = distanceToEnemy;
            nearestEnemy = enemy;
        }

        currentTarget = nearestEnemy != null && shortestDistance <= aimRange ? nearestEnemy.transform : null;
    }

    private void RotateToTarget()
    {
        if (currentTarget == null) return;
        
        var direction = currentTarget.position - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0, 0, angle - 90);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
    }

    private void ShootEnemy()
    {
        if (currentTarget == null) return;
        
        var currentBulletPrefab = bullet;
        
        if (playerInventory.HasBlastShot() && shotsFiredCounter + 1 >= shotsUntilBlastShot)
        {
            currentBulletPrefab = blastBullet; 
            shotsFiredCounter = 0; 
        }
        else
        {
            shotsFiredCounter++;
        }
        
        var pierceCount = 1; 
    
        if (playerInventory.HasPierceShot())
        {
            var pierceLevel = playerInventory.GetCurrentLevel(SpecialAbilityType.PierceShot);
            
            pierceCount = pierceLevel switch
            {
                SpecialAbilityLevel.Level01 => 2, 
                SpecialAbilityLevel.Level02 => 3,  
                SpecialAbilityLevel.Level03 => 4, 
                _ => pierceCount
            };
        }
        
        var currentBullet = Instantiate(currentBulletPrefab, bulletSpawn.position, Quaternion.identity);
        currentBullet.GetComponent<Bullet>().SeekTarget(currentTarget, currentDamage, pierceCount, playerInventory);
            
        SFXManager.Instance.ShootSound();

        if (DOTween.IsTweening(transform)) return;
        
        transform.DOScale(1.1f, 0.15f).OnComplete(() =>
        {
            transform.DOScale(1, 0.15f);
        });
    }

    private void RemoveEnemy(Enemy enemyToRemove)
    {
        enemiesInRange.Remove(enemyToRemove);
        enemyToRemove.OnBeingKilled -= GiveExperience;
        enemyToRemove.OnDeath -= RemoveEnemy;
    }

    private void GiveExperience(float experience, float coinsToAdd)
    {
        playerStats.GainExperience(experience);
        economyManager.AddCurrency(coinsToAdd);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy currentEnemy))
        {
            enemiesInRange.Add(currentEnemy);
            currentEnemy.OnBeingKilled += GiveExperience;
            currentEnemy.OnDeath += RemoveEnemy;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemyTheLeft))
        {
            RemoveEnemy(enemyTheLeft);
            enemyTheLeft.OnBeingKilled -= GiveExperience;
            enemyTheLeft.OnDeath -= RemoveEnemy;
        }
    }
    
    private void HandleSatelliteRespawn()
    {
        StartCoroutine(SatelliteRespawnCoroutine());
    }

    private IEnumerator SatelliteRespawnCoroutine()
    {
        yield return new WaitForSeconds(5f); 
        SpawnSatellite();
    }
    
    private void OnEnable()
    {
        PlayerAbilityInventory.OnSatelliteUnlocked += SpawnSatellite;
        Satellite.OnSatelliteDestroyed += HandleSatelliteRespawn;
    }

    private void OnDisable()
    {
        PlayerAbilityInventory.OnSatelliteUnlocked -= SpawnSatellite;
        Satellite.OnSatelliteDestroyed -= HandleSatelliteRespawn;
    }
}
