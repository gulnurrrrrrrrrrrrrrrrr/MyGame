using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;

    void Start() { Time.timeScale = 1f; }   

    public void SetPaused(bool paused)
    {
        pausePanel.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;
    }

    public void Pause() { SetPaused(true); }
    public void Resume() { SetPaused(false); }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}