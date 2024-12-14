using UnityEngine;

namespace Enemies
{
    public class ShootingEnemy : Enemy
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform shootingPoint;
        [SerializeField] private float shootingRange = 2f;
        [SerializeField] private float shootingCooldown = 3f;
        
        private float shootingTimer;

        private new void Update()
        {
            base.Update();
            
            if (target != null && Vector2.Distance(transform.position, target.position) <= shootingRange)
            {
                shootingTimer += Time.deltaTime;

                if (!(shootingTimer >= shootingCooldown)) return;
                
                Shoot();
                shootingTimer = 0f;
            }
        }
        
        protected override void SeekTarget()
        {
            if (target == null) return;
            
            if (Vector2.Distance(transform.position, target.position) <= shootingRange)
            {
                rb2d.linearVelocity = Vector2.zero;
            }
            else
            {
                Vector2 direction = (target.position - transform.position).normalized;
                rb2d.linearVelocity = direction * currentSpeed;
            }
        }

        private void Shoot()
        {
            var bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
            var bulletScript = bullet.GetComponent<EnemyBullet>();
            

            bulletScript.SeekPlayer(target, enemyData.Damage);
        }
    }
}
