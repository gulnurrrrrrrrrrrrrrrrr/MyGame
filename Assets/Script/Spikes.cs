using UnityEngine;

public class Spikes : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerStay2D(Collider2D other)
    {
        Health h = other.GetComponent<Health>();
        if (h != null) h.TakeDamage(damage);
    }
}