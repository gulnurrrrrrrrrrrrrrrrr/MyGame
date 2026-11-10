using UnityEngine;

public class ChargerAI : EnemyBase
{
    enum State { Idle, WindUp, Dash, Stunned }
    State state;

    [Header("Trigger")]
    [SerializeField] float triggerRange = 6f;
    [SerializeField] float cooldown = 1f;

    [Header("Charge")]
    [SerializeField] float windUp = 0.7f;
    [SerializeField] float dashSpeed = 9f;
    [SerializeField] float dashTime = 0.6f;
    [SerializeField] float hitRadius = 0.8f;
    [SerializeField] float stunTime = 1f;
    [SerializeField] Color stunColor = new Color(0.5f, 0.5f, 1f);

    float dashDir;
    bool hitDone;
    float cooldownLeft;

    protected override void Start()
    {
        base.Start();
        SetState(State.Idle);
    }

    void Update()
    {
        if (player == null) return;
        timer += Time.deltaTime;
        cooldownLeft -= Time.deltaTime;

        switch (state)
        {
            case State.Idle:
                Patrol();                                  // ходит между точками
                if (vision.CanSee && DistToPlayer < triggerRange && cooldownLeft <= 0f && OnScreen)
                    SetState(State.WindUp);
                break;

            case State.WindUp:
                moveX = 0f;
                if (timer >= windUp) SetState(State.Dash);
                break;

            case State.Dash:
                moveX = dashDir * dashSpeed;
                if (!hitDone && DistToPlayer < hitRadius)
                {
                    HitPlayer();
                    hitDone = true;
                }
                if (timer >= dashTime || WallAhead()) SetState(State.Stunned);
                break;

            case State.Stunned:
                moveX = 0f;
                if (timer >= stunTime)
                {
                    cooldownLeft = cooldown;
                    SetState(State.Idle);
                }
                break;
        }
    }

    bool WallAhead()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(dashDir, 0f), 0.7f, vision.ObstacleMask);
        return hit.collider != null;
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        Warn(next == State.WindUp);
        ShowState(next.ToString());
        if (next == State.WindUp)
        {
            dashDir = DirToPlayer;
            Face(dashDir);
        }
        if (next == State.Dash) hitDone = false;
        if (next == State.Stunned) sr.color = stunColor;
    }
}