using UnityEngine;

public class Hazard : MonoBehaviour
{
    protected virtual void Start()
    {
        Destroy(gameObject, 10f);   
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (!c.gameObject.CompareTag("Player")) return;
        OnHitPlayer(c.gameObject.GetComponent<Health>());
        Destroy(gameObject);
    }

    protected virtual void OnHitPlayer(Health player)
    {
        player.TakeDamage(1);
    }
}