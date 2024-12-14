using System;
using Enemies;
using UnityEngine;

public class Satellite : MonoBehaviour
{
    [SerializeField] private float orbitRadius = 3f; 
    [SerializeField] private float orbitSpeed = 50f; 
    
    public static event Action OnSatelliteDestroyed;

    private Transform player;

    private float damage;

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"));
    }
    
    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
        
        transform.position = player.position + Vector3.right * orbitRadius;
    }

    public void SetSatelliteParameters(SpecialAbilityLevel level)
    {
        damage = level switch
        {
            SpecialAbilityLevel.Level01 => 50f,
            SpecialAbilityLevel.Level02 => 75f,
            SpecialAbilityLevel.Level03 => 100f,
            _ => 0f
        };
    }
    

    private void Update()
    {
        if (player == null) return;
        
        transform.RotateAround(player.position, Vector3.forward, orbitSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
            OnSatelliteDestroyed?.Invoke();
            
            Destroy(gameObject);
        }
    }
}
