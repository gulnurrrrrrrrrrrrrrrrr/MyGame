
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public void SetMusicVolume(float value)
    {
        AudioListener.volume = value;

        if (SaveManager.Instance != null)
            SaveManager.Instance.SetMusicVolume(value);
    }

    public void SetSfxVolume(float value)
    {
        if (SaveManager.Instance != null)
            SaveManager.Instance.SetSfxVolume(value);
    }
    public void LoadSettings(
    UnityEngine.UI.Slider musicSlider,
    UnityEngine.UI.Slider sfxSlider)
    {
        if (SaveManager.Instance == null)
            return;

        musicSlider.SetValueWithoutNotify(
            SaveManager.Instance.Data.musicVolume
        );

        sfxSlider.SetValueWithoutNotify(
            SaveManager.Instance.Data.sfxVolume
        );
    }
}
