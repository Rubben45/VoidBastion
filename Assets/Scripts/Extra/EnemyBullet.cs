using System;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;
    private float damageToDeal;
    
    public void SeekPlayer(Transform player, float damage)
    {
        damageToDeal = damage;
        
        var direction = (player.position - transform.position).normalized;
        
        rb2d.linearVelocity = direction.normalized * 10f;
      
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0, 0, angle - 90);

        transform.rotation = rotation;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out PlayerStats playerStats))
        {
            playerStats.GetInjured(damageToDeal);
            Destroy(gameObject);
        }
    }
}
