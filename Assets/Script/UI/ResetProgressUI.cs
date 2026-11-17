
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetProgressUI : MonoBehaviour
{
    [SerializeField] private GameObject confirmPanel;

    void Start()
    {
        confirmPanel.SetActive(false);
    }

    public void AskReset()
    {
        confirmPanel.SetActive(true);
    }

    public void Cancel()
    {
        confirmPanel.SetActive(false);
    }

    public void ConfirmReset()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.ResetToDefaults();
        }

        confirmPanel.SetActive(false);

        SceneManager.LoadScene("MainMenu");
    }
}
