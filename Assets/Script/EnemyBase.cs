using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Vision2D))]
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] protected bool spriteFacesRight = true;
    [SerializeField] protected Color warnColor = Color.red;
    [SerializeField] protected AudioClip warnClip;
    [SerializeField] protected int damage = 1;
    [SerializeField] TMP_Text stateLabel;

    [Header("Patrol (общий для всех)")]
    [SerializeField] protected Transform[] points;     
    [SerializeField] protected float patrolSpeed = 1.5f;

    [Header("Edge check (не падать с края)")]
    [SerializeField] protected LayerMask groundMask;   
    [SerializeField] float edgeCheckForward = 0.6f;
    [SerializeField] float edgeCheckDown = 1.5f;

    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Vision2D vision;
    protected AudioSource audioSrc;
    protected Transform player;
    protected float timer;
    protected float moveX;

    Color normalColor;
    Camera cam;
    int pointIndex;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        vision = GetComponent<Vision2D>();
        audioSrc = GetComponent<AudioSource>();
        normalColor = sr.color;
    }

    protected virtual void Start()
    {
        player = vision.Player;
        cam = Camera.main;
    }

    protected float DistToPlayer => Vector2.Distance(transform.position, player.position);
    protected float DirToPlayer => Mathf.Sign(player.position.x - transform.position.x);

    protected bool OnScreen
    {
        get
        {
            if (cam == null) cam = Camera.main;
            Vector3 v = cam.WorldToViewportPoint(transform.position);
            return v.x > 0.05f && v.x < 0.95f && v.y > 0f && v.y < 1f;
        }
    }

    protected void Face(float dir)
    {
        if (dir == 0f) return;
        vision.Facing = dir;
        sr.flipX = spriteFacesRight ? dir < 0f : dir > 0f;
    }

    protected void Warn(bool on)
    {
        sr.color = on ? warnColor : normalColor;
        if (on && warnClip != null && audioSrc != null) audioSrc.PlayOneShot(warnClip);
    }

    protected void ShowState(string s)
    {
        if (stateLabel != null) stateLabel.text = s;
    }

    protected void HitPlayer()
    {
        player.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
    }


    protected void Patrol()
    {
        if (points == null || points.Length == 0) { moveX = 0f; return; }
        float dx = points[pointIndex].position.x - transform.position.x;
        float dir = Mathf.Sign(dx);

        
        if (Mathf.Abs(dx) < 0.15f || !GroundAhead(dir))
        {
            pointIndex = (pointIndex + 1) % points.Length;
            moveX = 0f;
            return;
        }

        Face(dir);
        moveX = dir * patrolSpeed;
    }


    protected bool GroundAhead(float dir)
    {
        Vector2 origin = (Vector2)transform.position + new Vector2(dir * edgeCheckForward, 0f);
        Debug.DrawRay(origin, Vector2.down * edgeCheckDown, Color.yellow);
        return Physics2D.Raycast(origin, Vector2.down, edgeCheckDown, groundMask).collider != null;
    }

    protected virtual void FixedUpdate()
    {
        float vx = moveX;
        if (vx != 0f && !GroundAhead(Mathf.Sign(vx))) vx = 0f;
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
    }
}