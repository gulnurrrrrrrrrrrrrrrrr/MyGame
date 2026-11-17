
using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemyStats",
    menuName = "Rabbit Adventure/Enemy Stats"
)]
public class EnemyStats : ScriptableObject
{
    [Header("Common")]
    public int damage = 1;
    public int maxHealth = 3;
    public float patrolSpeed = 1.5f;
    public float visionRange = 6f;
    public float fieldOfView = 120f;

    [Header("Chase")]
    public float chaseSpeed = 4f;
    public float loseTime = 2f;

    [Header("Melee Attack")]
    public float attackRange = 1.2f;
    public float strikeReach = 1.5f;
    public float windUp = 0.5f;
    public float recovery = 0.7f;
    public float attackCooldown = 1.2f;

    [Header("Charger")]
    public float triggerRange = 6f;
    public float dashSpeed = 9f;
    public float dashTime = 0.6f;
    public float hitRadius = 0.8f;
    public float stunTime = 1f;

    [Header("Shooter")]
    public float preferredDistance = 5f;
    public float distanceBand = 0.7f;
    public float moveSpeed = 2f;
    public float aimTime = 0.6f;
    public float shotCooldown = 1.5f;
    public float projectileSpeed = 5f;
}
