using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected EnemySO enemyData;
        [SerializeField] private GameObject deathParticles;

        [SerializeField] private GameObject healthUI;
        [SerializeField] private Image healthBar;
        
        [SerializeField] private GameObject rotatoryPart;
        
        [SerializeField] private int unlockLevel;
         
        public event Action<Enemy> OnDeath;
        public event Action<float, float> OnBeingKilled;
        
        public Transform target { get; set; }
        
        public int UnlockLevel => unlockLevel;
  
        private float currentHealth;
        protected Rigidbody2D rb2d;
        
        protected float currentSpeed;
        private bool isSlowed;

        private void Start()
        {
            currentHealth = enemyData.Health;
            rb2d = GetComponent<Rigidbody2D>();

            currentSpeed = enemyData.Speed;
        }

        public void Update()
        {
            SeekTarget();
            CheckEnemyRotation();
            CheckHealthBar();
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0) Die();
        }
        
        protected virtual void SeekTarget()
        {
            if (target == null) return;
            
            Vector2 direction = target.position - transform.position;
            
            rb2d.linearVelocity = direction.normalized * currentSpeed;
        }
        
        
        private void CheckEnemyRotation()
        {
            switch (enemyData.CurrentEnemyType)
            {
                case EnemyType.Asteroid:
                    rotatoryPart.transform.Rotate(Vector3.forward, .5f);
                    break;
                case EnemyType.Rocket:
                {
                    var direction = target.position - rotatoryPart.transform.position;
                    var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    var rotation = Quaternion.Euler(0, 0, angle - 90);
        
                    rotatoryPart.transform.rotation = Quaternion.Slerp(rotatoryPart.transform.rotation, rotation, Time.deltaTime * 10f);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckHealthBar()
        {
            if (currentHealth >= enemyData.Health) healthUI.SetActive(false);
            else
            {
                healthUI.SetActive(true);
                
                var targetFillAmount = currentHealth / enemyData.Health;
                healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, targetFillAmount, Time.deltaTime * 5f);
            }
        }
        
        public void ApplySlowEffect(float slowMultiplier, float duration)
        {
            if (isSlowed) return;

            isSlowed = true;
            currentSpeed *= slowMultiplier;
           
            StartCoroutine(RemoveSlowEffect(duration));
        }

        private IEnumerator RemoveSlowEffect(float duration)
        {
            yield return new WaitForSeconds(duration);
            currentSpeed = enemyData.Speed;
            isSlowed = false;
        }
        
        private void Die()
        {
            var currentDeathParticles = Instantiate(deathParticles, transform.position, Quaternion.identity);
            currentDeathParticles.GetComponent<ParticleSystem>().Play();
            
            OnBeingKilled?.Invoke(enemyData.Experience, enemyData.CoinsReward);
            OnDeath?.Invoke(this);
            
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out PlayerStats playerStats))
            {
                playerStats.GetInjured(enemyData.Damage);
                OnDeath?.Invoke(this);
                Destroy(gameObject);
            }
        }
    }
}
