
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] protected ItemData itemData;

    protected virtual void Start()
    {
        float lifetime = itemData != null ? itemData.lifetime : 10f;
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (!c.gameObject.CompareTag("Player")) return;

        Health player = c.gameObject.GetComponent<Health>();

        OnHitPlayer(player);

        Destroy(gameObject);
    }

    protected virtual void OnHitPlayer(Health player)
    {
        if (player == null || itemData == null)
            return;

        switch (itemData.itemType)
        {
            case ItemType.Heart:
                player.Heal(itemData.healAmount);
                player.PlayPickup();
                break;

            case ItemType.Hazard:
                player.TakeDamage(itemData.damageAmount);
                break;
        }
    }
}
