
using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerStats",
    menuName = "Rabbit Adventure/Player Stats"
)]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 12f;

    [Header("Health")]
    public int maxHealth = 5;
    public int startingHealth = 3;

    [Header("Shooting")]
    public float shootCooldown = 0.3f;
    public float bulletSpeed = 12f;
}
