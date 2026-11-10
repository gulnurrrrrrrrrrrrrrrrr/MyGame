using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyShot : MonoBehaviour
{
    [SerializeField] float lifeTime = 4f;      
    [SerializeField] LayerMask wallMask;       

    int damage = 1;

    public void Launch(Vector2 velocity, int dmg)
    {
        damage = dmg;
        GetComponent<Rigidbody2D>().linearVelocity = velocity;
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player2D p = other.GetComponentInParent<Player2D>();
        if (p != null)
        {
            Health h = p.GetComponent<Health>();
            if (h != null) h.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
        if (((1 << other.gameObject.layer) & wallMask) != 0) Destroy(gameObject);
    }
}