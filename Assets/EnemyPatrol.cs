using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float distance = 2f;
    public float speed = 2f;
    float startX, lastX;
    SpriteRenderer sr;

    void Start()
    {
        startX = transform.position.x;
        lastX = startX;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float x = startX + Mathf.PingPong(Time.time * speed, distance * 2f) - distance;
        sr.flipX = x < lastX;     
        lastX = x;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}