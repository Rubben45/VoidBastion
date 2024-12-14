using UnityEngine;
using Enemies;

public class BlastBullet : Bullet
{
    [SerializeField] private float blastRadius = 3f; 
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject blastParticle;

    protected override void  OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Enemy _))
        {
            DealAreaDamage();
            Destroy(gameObject);
        }
    }

    private void DealAreaDamage()
    {
        var enemiesHit = Physics2D.OverlapCircleAll(transform.position, blastRadius, enemyLayer);
        foreach (var enemyCollider in enemiesHit)
        {
            if (enemyCollider.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(damageToDeal);
                
                var instantiatedParticles = Instantiate(blastParticle, transform.position, Quaternion.identity);
                instantiatedParticles.GetComponent<ParticleSystem>().Play();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
