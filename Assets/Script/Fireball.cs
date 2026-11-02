using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Vector2 velocity;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = velocity;
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Health h = other.GetComponent<Health>();
        if (h != null) { h.TakeDamage(1); Destroy(gameObject); }
        else if (other.CompareTag("Ground")) Destroy(gameObject);
    }
}