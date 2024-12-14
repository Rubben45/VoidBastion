using System;
using Enemies;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Transform[] spawnPoints;
    
    [SerializeField] private float initialSpawnRate = 1f;
    
    [SerializeField] private float spawnRateReductionPerLevel = 0.2f;
    [SerializeField] private float minSpawnRate = 0.5f;

    private float spawnRate;
    private float nextSpawn;
    private int currentLevel = 1;
    private Dictionary<int, List<GameObject>> levelUnlockedEnemies;

    private void Start()
    {
        InitializeEnemyLevels();
        spawnRate = initialSpawnRate;
        
        
        PlayerStats.OnLevelChanged += HandleLevelUp;
    }
    
    private void InitializeEnemyLevels()
    {
        levelUnlockedEnemies = new Dictionary<int, List<GameObject>>();

        foreach (var existingEnemy in enemies)
        {
            var enemy = existingEnemy.GetComponent<Enemy>();

            var unlockLevel = enemy.UnlockLevel;
            if (!levelUnlockedEnemies.ContainsKey(unlockLevel))
            {
                levelUnlockedEnemies[unlockLevel] = new List<GameObject>();
            }

            levelUnlockedEnemies[unlockLevel].Add(existingEnemy);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        nextSpawn += Time.deltaTime;

        if (nextSpawn < spawnRate) return;
        
        SpawnEnemy();
        nextSpawn = 0f;
    }

    private void SpawnEnemy()
    {
        if (!levelUnlockedEnemies.ContainsKey(currentLevel)) return;

        var availableEnemies = new List<GameObject>();

        foreach (var level in levelUnlockedEnemies.Keys.Where(level => level <= currentLevel))
        {
            availableEnemies.AddRange(levelUnlockedEnemies[level]);
        }

        var randomPointIndex = Random.Range(0, spawnPoints.Length);
        var randomEnemyIndex = Random.Range(0, availableEnemies.Count);

        var instantiatedEnemy = Instantiate(availableEnemies[randomEnemyIndex], spawnPoints[randomPointIndex].position, Quaternion.identity);
        instantiatedEnemy.GetComponent<Enemy>().target = playerTransform;
    }
    
    private void HandleLevelUp(int newLevel)
    {
        currentLevel = newLevel;

        spawnRate = Mathf.Max(minSpawnRate, initialSpawnRate - (spawnRateReductionPerLevel * (currentLevel - 1)));
    }

    private void OnDestroy()
    {
        PlayerStats.OnLevelChanged -= HandleLevelUp;
    }
}
