using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Hazard[] prefabs;        
    public float firstDelay = 1f;
    public float interval = 2f;
    public float xRange = 8f;
    public float spawnY = 6f;

    void Start()
    {
        InvokeRepeating(nameof(Spawn), firstDelay, interval);
    }

    void Spawn()
    {
        Hazard pick = prefabs[Random.Range(0, prefabs.Length)];
        Instantiate(pick, new Vector3(Random.Range(-xRange, xRange), spawnY, 0), Quaternion.identity);
    }
}