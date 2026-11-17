using UnityEngine;

public class ShooterAI : EnemyBase
{
    enum State { Idle, KeepDistance, Aim, Cooldown }
    State state;

    [Header("Distance")]
    [SerializeField] Transform leftLimit;
    [SerializeField] Transform rightLimit;

    [Header("Attack")]
    [SerializeField] float shotSpeed = 5f;
    [SerializeField] EnemyShot shotPrefab;
    [SerializeField] Transform muzzle;
    [SerializeField] LineRenderer laser;
    [SerializeField] float laserLength = 12f;

    Vector2 aimDir;

    protected override void Start()
    {
        base.Start();
        SetState(State.Idle);
    }

    void Update()
    {
        if (player == null) return;
        timer += Time.deltaTime;

        switch (state)
        {
            case State.Idle:
                Patrol();                                  
                if (vision.CanSee) SetState(State.KeepDistance);
                break;

            case State.KeepDistance:
                if (vision.CanSee) timer = 0f;
                Face(DirToPlayer);
                float d = DistToPlayer;
                if (d < enemyStats.preferredDistance - enemyStats.distanceBand) moveX = -DirToPlayer * enemyStats.moveSpeed;
                else if (d > enemyStats.preferredDistance + enemyStats.distanceBand) moveX = DirToPlayer * enemyStats.moveSpeed;
                else
                {
                    moveX = 0f;
                    if (vision.CanSee && OnScreen) SetState(State.Aim);
                }
                ClampToLimits();
                if (timer > enemyStats.loseTime) SetState(State.Idle);
                break;

            case State.Aim:
                moveX = 0f;
                if (timer >= enemyStats.aimTime)
                {
                    Fire();
                    SetState(State.Cooldown);
                }
                break;

            case State.Cooldown:
                moveX = 0f;
                if (timer >= enemyStats.attackCooldown)
                    SetState(vision.CanSee ? State.KeepDistance : State.Idle);
                break;
        }
    }

    void ClampToLimits()
    {
        if (leftLimit != null && moveX < 0f && transform.position.x <= leftLimit.position.x) moveX = 0f;
        if (rightLimit != null && moveX > 0f && transform.position.x >= rightLimit.position.x) moveX = 0f;
    }

    void Fire()
    {
        EnemyShot shot = Instantiate(shotPrefab, muzzle.position, Quaternion.identity);
        shot.Launch(
            aimDir * enemyStats.projectileSpeed,
            enemyStats.damage
        );
    }

    void UpdateLaser()
    {
        Vector2 start = muzzle.position;
        Vector2 end = start + aimDir * laserLength;
        RaycastHit2D hit = Physics2D.Raycast(start, aimDir, laserLength, vision.ObstacleMask);
        if (hit.collider != null) end = hit.point;
        laser.SetPosition(0, start);
        laser.SetPosition(1, end);
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        bool aiming = next == State.Aim;
        Warn(aiming);
        ShowState(next.ToString());
        if (laser != null) laser.enabled = aiming;
        if (aiming)
        {
            aimDir = ((Vector2)player.position - (Vector2)muzzle.position).normalized;
            if (laser != null) UpdateLaser();
        }
    }
}