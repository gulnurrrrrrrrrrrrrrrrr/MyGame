using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 5f;
    public float minX = -10f;
    public float maxX = 10f;

    void LateUpdate()
    {
        float x = Mathf.Clamp(target.position.x, minX, maxX);
        Vector3 goal = new Vector3(x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, goal, smooth * Time.deltaTime);
    }
}