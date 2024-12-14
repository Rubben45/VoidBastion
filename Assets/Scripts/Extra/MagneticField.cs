using UnityEngine;
using System.Collections;
using Enemies;

public class MagneticField : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject magneticFieldParticle;
    
    private float damage = 10f;      
    private const float effectRadius = 2f;  
    private float cooldownTime = 5f;  
    
    private float magneticFieldTimer;

    private bool isCooldown;
    private PlayerAbilityInventory playerInventory;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerAbilityInventory>();
        SetAbilityLevelParameters();
    }

    private void SetAbilityLevelParameters()
    {
        var abilityLevel = playerInventory.GetCurrentLevel(SpecialAbilityType.MagneticField);

        // Set damage and cooldown based on ability level
        switch (abilityLevel)
        {
            case SpecialAbilityLevel.Level01:
                damage = 10f;
                cooldownTime = 6f;
                break;
            case SpecialAbilityLevel.Level02:
                damage = 20f;
                cooldownTime = 5f;
                break;
            case SpecialAbilityLevel.Level03:
                damage = 30f;
                cooldownTime = 4f;
                break;
        }
    }
    
    private void Update()
    {
        if (isCooldown || !playerInventory.HasMagneticField()) return;

        var enemiesInRange = Physics2D.OverlapCircleAll(transform.position, effectRadius, enemyLayer);
        if (enemiesInRange.Length > 0) 
        {
            ActivateMagneticField();
            StartCoroutine(CooldownRoutine());
        }
    }

    private void ActivateMagneticField()
    {
        var enemiesInRange = Physics2D.OverlapCircleAll(transform.position, effectRadius, enemyLayer);
        SFXManager.Instance.MagFieldSound();
        
        foreach (var enemyCollider in enemiesInRange)
        {
            if (enemyCollider.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
            }
        }

        // Optional: Add visual or sound effect here for activation
        var instantiatedMagneticField = Instantiate(magneticFieldParticle, transform.position, Quaternion.identity);
        instantiatedMagneticField.GetComponent<ParticleSystem>().Play();
        
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
}
