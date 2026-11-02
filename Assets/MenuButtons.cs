using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void PlayGame() { SceneManager.LoadScene("Level1"); }
    public void LoadMenu() { SceneManager.LoadScene("MainMenu"); }
    public void QuitGame() { Application.Quit(); }
}