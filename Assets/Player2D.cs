using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player2D : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    public Animator gateAnimator;
    public AudioClip jumpClip;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    Shooter shooter;
    Health health;
    AudioSource src;

    Vector2 move;
    float facing = 1f;
    bool onGround;
    int coins;
    int coinsTotal;

    public int Coins => coins;
    public int CoinsTotal => coinsTotal;
    public bool AllCoinsCollected => coins >= coinsTotal;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        shooter = GetComponent<Shooter>();
        health = GetComponent<Health>();
        src = GetComponent<AudioSource>();
        if (health != null && playerStats != null)
{
    health.Configure(
        playerStats.maxHealth,
        playerStats.startingHealth
    );
}

if (shooter != null && playerStats != null)
{
    shooter.Configure(playerStats);
}
    }

    void Start()
    {
        coinsTotal = GameObject.FindGameObjectsWithTag("Coin").Length;

        if (LevelLoader.Instance != null &&
            LevelLoader.Instance.Data != null &&
            LevelLoader.Instance.Data.requiredCoins > 0)
        {
            coinsTotal = LevelLoader.Instance.Data.requiredCoins;
        }
    }

    public void OnMove(InputValue value) { move = value.Get<Vector2>(); }

    public void OnJump()
    {
        if (onGround)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                playerStats.jumpForce
            ); onGround = false;
            if (src != null && jumpClip != null) src.PlayOneShot(jumpClip);
        }
    }

    public void OnFire()
    {
        if (shooter != null) shooter.Shoot(facing);
    }

    void Update()
    {
        anim.SetFloat("Speed", Mathf.Abs(move.x));
        anim.SetBool("Grounded", onGround);

        if (move.x > 0.01f) { sr.flipX = false; facing = 1f; }
        else if (move.x < -0.01f) { sr.flipX = true; facing = -1f; }

        if (transform.position.y < -10f)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            move.x * playerStats.moveSpeed,
            rb.linearVelocity.y
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coins++;
            if (health != null) health.PlayPickup();
            if (coins >= coinsTotal && gateAnimator != null)
                gateAnimator.SetBool("Open", true);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")) onGround = true;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && health != null)
            health.TakeDamage(1);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")) onGround = false;
    }
    public void ApplyStats(PlayerStats stats)
    {
        if (stats == null)
            return;

        playerStats = stats;

        if (health != null)
        {
            health.Configure(
                playerStats.maxHealth,
                playerStats.startingHealth
            );
        }

        if (shooter != null)
        {
            shooter.Configure(playerStats);
        }
    }
}