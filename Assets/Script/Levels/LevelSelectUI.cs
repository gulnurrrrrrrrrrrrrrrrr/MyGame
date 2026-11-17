
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;

    void Start()
    {
        RefreshButtons();
    }

    public void RefreshButtons()
    {
        if (SaveManager.Instance == null)
            return;

        var unlocked =
            SaveManager.Instance.Data.unlockedLevels;

        level1Button.interactable = unlocked.Contains(1);
        level2Button.interactable = unlocked.Contains(2);
        level3Button.interactable = unlocked.Contains(3);
    }

    public void OpenLevel1()
    {
        OpenLevel(1, "Level1");
    }

    public void OpenLevel2()
    {
        OpenLevel(2, "Level2");
    }

    public void OpenLevel3()
    {
        OpenLevel(3, "Level3");
    }

    void OpenLevel(int level, string scene)
    {
        if (SaveManager.Instance == null)
            return;

        if (!SaveManager.Instance.Data.unlockedLevels.Contains(level))
            return;

        SceneManager.LoadScene(scene);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
