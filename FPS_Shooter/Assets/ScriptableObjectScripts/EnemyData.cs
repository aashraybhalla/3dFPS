using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float speed;

    public float chaseRadius;
    public float attackRange;
    public float timeInIdle;
    public float patrolSpeed;
    public float chaseSpeed;
    public int damage;
    public float maxHealth;
    public Transform player;
}
