
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private LevelData levelData;

    public static LevelLoader Instance { get; private set; }

    public LevelData Data => levelData;

    void Awake()
    {
        Instance = this;

        if (levelData == null)
        {
            Debug.LogError("LevelData is missing!");
        }
    }

    void Start()
    {
        if (levelData == null)
            return;
        Player2D player = FindFirstObjectByType<Player2D>();

        if (player != null && levelData.playerStats != null)
        {
            player.ApplyStats(levelData.playerStats);
        }

        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        foreach (EnemySpawnGroup group in levelData.enemyGroups)
        {
            if (group.enemyPrefab == null ||
                group.spawnPositions == null)
                continue;

            foreach (Vector2 position in group.spawnPositions)
            {
                Instantiate(
                    group.enemyPrefab,
                    position,
                    Quaternion.identity
                );
            }
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
