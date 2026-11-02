using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float cooldown = 0.3f;
    public AudioClip shootClip;

    float nextShot;
    AudioSource src;

    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    public void Shoot(float dir)
    {
        if (Time.time < nextShot) return;
        nextShot = Time.time + cooldown;

        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        b.GetComponent<Bullet>().direction = dir;

        if (src != null && shootClip != null) src.PlayOneShot(shootClip);
    }
}