using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public string nextScene;

    void OnTriggerEnter2D(Collider2D other)
    {
        Player2D p = other.GetComponent<Player2D>();
        if (p != null && p.AllCoinsCollected)
            SceneManager.LoadScene(nextScene);
    }
}