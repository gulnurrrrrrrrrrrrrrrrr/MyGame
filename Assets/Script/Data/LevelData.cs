
using UnityEngine;

[System.Serializable]
public class EnemySpawnGroup
{
    public GameObject enemyPrefab;
    public Vector2[] spawnPositions;
}

[CreateAssetMenu(
    fileName = "LevelData",
    menuName = "Rabbit Adventure/Level Data"
)]
public class LevelData : ScriptableObject
{
    [Header("Level")]
    public int levelNumber = 1;
    public string sceneName = "Level1";

    [Header("Goal")]
    public int requiredCoins = 5;
    public float timeLimit = 120f;

    [Header("Player")]
    public PlayerStats playerStats;

    [Header("Enemies")]
    public EnemySpawnGroup[] enemyGroups;
}
