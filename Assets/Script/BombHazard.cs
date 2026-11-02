using UnityEngine;

public class BombHazard : Hazard
{
    public GameObject explosionPrefab;   
    public AudioClip explosionClip;

    protected override void Start()
    {
        base.Start();
        Invoke(nameof(Explode), 2f);     
    }

    void Explode()
    {
        if (explosionPrefab != null)
        {
            GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 1f);
        }
        if (explosionClip != null)
            AudioSource.PlayClipAtPoint(explosionClip, transform.position);
        Destroy(gameObject);
    }

    protected override void OnHitPlayer(Health p)
    {
        CancelInvoke();       
        p.TakeDamage(2);
    }
}