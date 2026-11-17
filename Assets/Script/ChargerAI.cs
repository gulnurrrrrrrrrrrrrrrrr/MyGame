
using UnityEngine;

public class ChargerAI : EnemyBase
{
    enum State { Idle, WindUp, Dash, Stunned }
    State state;

    [Header("Charge")]
    [SerializeField] Color stunColor = new Color(0.5f, 0.5f, 1f);

    float dashDir;
    bool hitDone;
    float cooldownLeft;

    protected override void Start()
    {
        base.Start();

        if (enemyStats == null)
        {
            Debug.LogError("EnemyStats is missing on Charger: " + gameObject.name);
            enabled = false;
            return;
        }

        SetState(State.Idle);
    }

    void Update()
    {
        if (player == null || enemyStats == null) return;

        timer += Time.deltaTime;

        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;

        switch (state)
        {
            case State.Idle:

                Patrol();

                if (vision != null &&
                    vision.CanSee &&
                    DistToPlayer < enemyStats.triggerRange &&
                    cooldownLeft <= 0f &&
                    OnScreen)
                {
                    SetState(State.WindUp);
                }

                break;

            case State.WindUp:

                moveX = 0f;

                if (timer >= enemyStats.windUp)
                {
                    SetState(State.Dash);
                }

                break;

            case State.Dash:

                moveX = dashDir * enemyStats.dashSpeed;

                if (!hitDone && DistToPlayer < enemyStats.hitRadius)
                {
                    HitPlayer();
                    hitDone = true;
                }

                if (timer >= enemyStats.dashTime || WallAhead())
                {
                    SetState(State.Stunned);
                }

                break;

            case State.Stunned:

                moveX = 0f;

                if (timer >= enemyStats.stunTime)
                {
                    cooldownLeft = enemyStats.attackCooldown;
                    SetState(State.Idle);
                }

                break;
        }
    }

    bool WallAhead()
    {
        if (vision == null) return false;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            new Vector2(dashDir, 0f),
            0.7f,
            vision.ObstacleMask
        );

        return hit.collider != null;
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;

        Warn(next == State.WindUp);

        if (next != State.Stunned && sr != null)
            sr.color = normalColor;
        ShowState(next.ToString());

        if (next == State.WindUp)
        {
            dashDir = DirToPlayer;
            Face(dashDir);
        }

        if (next == State.Dash)
        {
            hitDone = false;
        }

        if (next == State.Stunned && sr != null)
        {
            sr.color = stunColor;
        }
    }

   
}
