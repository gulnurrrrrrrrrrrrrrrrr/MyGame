using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 12f;
    public float direction = 1f;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(direction * speed, 0f);
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Boss boss = other.GetComponent<Boss>();
        if (boss != null) { boss.TakeHit(); Destroy(gameObject); }
    }
}