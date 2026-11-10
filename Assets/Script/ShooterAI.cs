using UnityEngine;

public class ShooterAI : EnemyBase
{
    enum State { Idle, KeepDistance, Aim, Cooldown }
    State state;

    [Header("Distance")]
    [SerializeField] float preferredDistance = 5f;
    [SerializeField] float distanceBand = 0.7f;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float loseTime = 2f;
    [SerializeField] Transform leftLimit;
    [SerializeField] Transform rightLimit;

    [Header("Attack")]
    [SerializeField] float aimTime = 0.6f;
    [SerializeField] float cooldown = 1.5f;
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
                Patrol();                                  // ходит между точками
                if (vision.CanSee) SetState(State.KeepDistance);
                break;

            case State.KeepDistance:
                if (vision.CanSee) timer = 0f;
                Face(DirToPlayer);
                float d = DistToPlayer;
                if (d < preferredDistance - distanceBand) moveX = -DirToPlayer * moveSpeed;
                else if (d > preferredDistance + distanceBand) moveX = DirToPlayer * moveSpeed;
                else
                {
                    moveX = 0f;
                    if (vision.CanSee && OnScreen) SetState(State.Aim);
                }
                ClampToLimits();
                if (timer > loseTime) SetState(State.Idle);
                break;

            case State.Aim:
                moveX = 0f;
                if (timer >= aimTime)
                {
                    Fire();
                    SetState(State.Cooldown);
                }
                break;

            case State.Cooldown:
                moveX = 0f;
                if (timer >= cooldown)
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
        shot.Launch(aimDir * shotSpeed, damage);
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