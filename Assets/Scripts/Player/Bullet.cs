using System;
using Enemies;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   private Rigidbody2D rb2d;

   protected float damageToDeal;
   
   private int pierceCount = 1; 
   
   private bool hasFreezeShot;
   private float slowPercentage;
   private float slowDuration;
   
   private Renderer bulletRenderer;
   
   public float DamageToDeal => damageToDeal;

   public virtual void Awake()
   {
      rb2d = GetComponent<Rigidbody2D>();
      bulletRenderer = GetComponent<Renderer>();
   }
   
   private void Update()
   {
      if (!bulletRenderer.isVisible)
      {
         Destroy(gameObject);
      }
   }

   public void SeekTarget(Transform targetToSeek, float bulletDamage, int currentPierceCount, PlayerAbilityInventory playerInventory)
   {
      damageToDeal = bulletDamage;
      pierceCount = currentPierceCount;
      
      if (playerInventory.HasFreezeShot())
      {
         hasFreezeShot = true;
         SetFreezeShotEffect(playerInventory.GetCurrentLevel(SpecialAbilityType.FreezeShot));
      }
      
      Vector2 direction = targetToSeek.position - transform.position;
      rb2d.linearVelocity = direction.normalized * 10f;
      
      var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      var rotation = Quaternion.Euler(0, 0, angle - 90);

      transform.rotation = rotation;
   }

   protected virtual void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.TryGetComponent(out Enemy enemy))
      {
         SFXManager.Instance.PlayHitSound();
         enemy.TakeDamage(damageToDeal);
         pierceCount--;
         
         if (hasFreezeShot)
         {
            enemy.ApplySlowEffect(slowPercentage, slowDuration);
         }

         if (pierceCount <= 0)
         {
            Destroy(gameObject);
         }
      }
   }
   
   private void SetFreezeShotEffect(SpecialAbilityLevel level)
   {
      switch (level)
      {
         case SpecialAbilityLevel.Level01:
            slowPercentage = 0.85f;
            slowDuration = 1f;
            break;
         case SpecialAbilityLevel.Level02:
            slowPercentage = 0.75f;
            slowDuration = 1f;
            break;
         case SpecialAbilityLevel.Level03:
            slowPercentage = 0.65f;
            slowDuration = 2f;
            break;
         default:
            slowPercentage = 1f;
            slowDuration = 0f;
            break;
      }
   }
}
