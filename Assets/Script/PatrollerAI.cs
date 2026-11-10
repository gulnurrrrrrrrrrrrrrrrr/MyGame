using UnityEngine;

public class PatrollerAI : EnemyBase
{
    enum State { Patrol, Chase, WindUp, Recovery }
    State state;

    [Header("Chase")]
    [SerializeField] float chaseSpeed = 4f;
    [SerializeField] float loseTime = 2f;

    [Header("Attack")]
    [SerializeField] float attackRange = 1.2f;
    [SerializeField] float strikeReach = 1.5f;
    [SerializeField] float windUp = 0.5f;
    [SerializeField] float recovery = 0.7f;
    [SerializeField] float cooldown = 1.2f;

    float cooldownLeft;

    protected override void Start()
    {
        base.Start();
        SetState(State.Patrol);
    }

    void Update()
    {
        if (player == null) return;
        timer += Time.deltaTime;
        cooldownLeft -= Time.deltaTime;

        switch (state)
        {
            case State.Patrol:
                Patrol();
                if (vision.CanSee) SetState(State.Chase);
                break;

            case State.Chase:
                if (vision.CanSee) timer = 0f;
                Face(DirToPlayer);
                moveX = DistToPlayer > attackRange ? DirToPlayer * chaseSpeed : 0f;
                if (DistToPlayer <= attackRange && cooldownLeft <= 0f && OnScreen)
                    SetState(State.WindUp);
                else if (timer > loseTime)
                    SetState(State.Patrol);
                break;

            case State.WindUp:
                moveX = 0f;
                if (timer >= windUp)
                {
                    if (DistToPlayer <= strikeReach) HitPlayer();
                    SetState(State.Recovery);
                }
                break;

            case State.Recovery:
                moveX = 0f;
                if (timer >= recovery)
                {
                    cooldownLeft = cooldown;
                    SetState(State.Chase);
                }
                break;
        }
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        Warn(next == State.WindUp);
        ShowState(next.ToString());
    }
}