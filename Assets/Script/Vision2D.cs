using UnityEngine;

public class Vision2D : MonoBehaviour
{
    [SerializeField] Transform player;          
    [SerializeField] float viewRange = 6f;     
    [SerializeField] float fov = 120f;         
    [SerializeField] LayerMask obstacleMask;    
    [SerializeField] float checkInterval = 0.15f; 
    public Transform Player => player;
    public LayerMask ObstacleMask => obstacleMask;
    public bool CanSee { get; private set; }
    public float Facing = 1f;                  

    float t;

    void Awake()
    {
        if (player == null)
        {
            Player2D p = FindFirstObjectByType<Player2D>();   
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        t -= Time.deltaTime;
        if (t > 0f) return;
        t = checkInterval;
        CanSee = Check();
    }

    bool Check()
    {
        if (player == null) return false;

        Vector2 eye = transform.position;
        Vector2 toPlayer = (Vector2)player.position - eye;

        if (toPlayer.magnitude > viewRange) return false;

        if (Vector2.Angle(new Vector2(Facing, 0f), toPlayer) > fov / 2f) return false;

        RaycastHit2D hit = Physics2D.Raycast(eye, toPlayer.normalized, toPlayer.magnitude, obstacleMask);
        bool seen = hit.collider == null;
        Debug.DrawRay(eye, toPlayer, seen ? Color.green : Color.red, checkInterval);
        return seen;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRange);
        float f = Application.isPlaying ? Facing : 1f;
        Vector3 fwd = new Vector3(f, 0f, 0f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, fov / 2f) * fwd * viewRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, -fov / 2f) * fwd * viewRange);
    }
}