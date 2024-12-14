using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "EnemySO", menuName = "Enemy/EnemySO")]
    public class EnemySO : ScriptableObject
    {
        [SerializeField] private EnemyType enemyType;
        [SerializeField] private float experience;
        [SerializeField] private float coinsReward;
        [SerializeField] private float health;
        [SerializeField] private float damage;
        [SerializeField] private float speed;
        
        public float CoinsReward => coinsReward;
        public float Health => health;
        public float Damage => damage;
        public float Speed => speed;
        public float Experience => experience;
        public EnemyType CurrentEnemyType => enemyType;
    }


    public enum EnemyType
    {
        Asteroid,
        Rocket
    }
}
