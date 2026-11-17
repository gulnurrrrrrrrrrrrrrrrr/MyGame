using UnityEngine;

public class Shooter : MonoBehaviour

{
    PlayerStats playerStats;

    public void Configure(PlayerStats stats)
    {
        playerStats = stats;
    }
    public GameObject bulletPrefab;
    [SerializeField] private float defaultCooldown = 0.3f; public AudioClip shootClip;

    float nextShot;
    AudioSource src;

    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    public void Shoot(float dir)
    {
        if (Time.time < nextShot) return;
        float currentCooldown = playerStats != null
            ? playerStats.shootCooldown
            : defaultCooldown;

        nextShot = Time.time + currentCooldown;
        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bullet = b.GetComponent<Bullet>();
        bullet.direction = dir;

        if (playerStats != null)
        {
            bullet.speed = playerStats.bulletSpeed;
        }
        if (src != null && shootClip != null) src.PlayOneShot(shootClip);
    }
}