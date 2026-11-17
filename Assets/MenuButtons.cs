using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    public void LoadMenu() { SceneManager.LoadScene("MainMenu"); }
    public void QuitGame() { Application.Quit(); }
}