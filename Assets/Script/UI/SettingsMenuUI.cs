
using UnityEngine;

public class SettingsMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}
