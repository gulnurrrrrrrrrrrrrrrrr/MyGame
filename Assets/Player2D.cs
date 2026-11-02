using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player2D : MonoBehaviour
{
    public float speed = 7f;
    public float jumpForce = 12f;
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
    }

    void Start()
    {
        coinsTotal = GameObject.FindGameObjectsWithTag("Coin").Length;
    }

    public void OnMove(InputValue value) { move = value.Get<Vector2>(); }

    public void OnJump()
    {
        if (onGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            onGround = false;
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
        rb.linearVelocity = new Vector2(move.x * speed, rb.linearVelocity.y);
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
}