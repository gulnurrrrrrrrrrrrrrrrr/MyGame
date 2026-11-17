
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public string nextScene;

    void OnTriggerEnter2D(Collider2D other)
    {
        Player2D player = other.GetComponent<Player2D>();

        if (player == null || !player.AllCoinsCollected)
            return;

        if (SaveManager.Instance != null &&
            LevelLoader.Instance != null)
        {
            SaveManager.Instance.CompleteLevel(
                LevelLoader.Instance.Data.levelNumber,
                player.Coins
            );
        }

        if (!string.IsNullOrEmpty(nextScene))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
