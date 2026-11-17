using UnityEngine;

public class PatrollerAI : EnemyBase
{
    enum State { Patrol, Chase, WindUp, Recovery }
    State state;

    [Header("Chase")]

    [Header("Attack")]

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
                moveX = DistToPlayer > enemyStats.attackRange
                    ? DirToPlayer * enemyStats.chaseSpeed
                    : 0f;
                if (DistToPlayer <= enemyStats.attackRange && cooldownLeft <= 0f && OnScreen)
                    SetState(State.WindUp);
                else if (timer > enemyStats.loseTime)
                    SetState(State.Patrol);
                break;

            case State.WindUp:
                moveX = 0f;
                if (timer >= enemyStats.windUp)
                {
                    if (DistToPlayer <= enemyStats.strikeReach) HitPlayer();
                    SetState(State.Recovery);
                }
                break;

            case State.Recovery:
                moveX = 0f;
                if (timer >= enemyStats.recovery)
                {
                    cooldownLeft = enemyStats.attackCooldown;
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