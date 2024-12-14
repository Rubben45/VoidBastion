using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

namespace Enemies
{
    public class AssasinEnemy : Enemy
    {
        [SerializeField] private Transform dodgeDetector;  
        [SerializeField] private float dodgeDistance = 2f;
        [SerializeField] private float dodgeSpeed = 10f;   
        
        [SerializeField] private GameObject disappearEffect;
        [SerializeField] private GameObject appearEffect;
        
        [SerializeField] private Collider2D enemyCollider;
        [SerializeField] private Collider2D detectionCollider;
        
        private bool isDodging;
        private bool hasDodged;

        // Aici detectăm gloanțele cu collider-ul detector
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out Bullet bullet) && !isDodging && !hasDodged)
            {
                Vector2 dodgeDirection = GetDodgeDirection(other.transform);
                StartCoroutine(DodgeWithEffect(dodgeDirection));
            }
        }
        
        private Vector2 GetDodgeDirection(Transform bulletTransform)
        {
            Vector2 toBullet = bulletTransform.position - transform.position;
            Vector2 dodgeDirection = Vector2.Perpendicular(toBullet).normalized; // perpendicular la glonț
            
            return Random.value > 0.5f ? dodgeDirection : -dodgeDirection;
        }

        private IEnumerator DodgeWithEffect(Vector2 dodgeDirection)
        {
            isDodging = true;
            hasDodged = true;
            
            enemyCollider.enabled = false;
            detectionCollider.enabled = false;

            // Efect de particule și ascundere vizuală
            Instantiate(disappearEffect, transform.position, Quaternion.identity);
            SFXManager.Instance.AssassinSound();
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null) spriteRenderer.enabled = false;

            yield return new WaitForSeconds(0.1f); // Delay scurt înainte de a se "teleporta"

            // Calculăm și aplicăm poziția finală
            Vector2 dodgeTarget = (Vector2)transform.position + dodgeDirection * dodgeDistance;
            transform.position = dodgeTarget;

            // Efect de particule la reapariție
            Instantiate(appearEffect, transform.position, Quaternion.identity);
            SFXManager.Instance.AssassinSound();
            if (spriteRenderer != null) spriteRenderer.enabled = true;
            
            enemyCollider.enabled = true;

            // Resetare isDodging și cooldown pentru hasDodged
            isDodging = false;
        }
    }
    
}
