
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public SaveData Data { get; private set; }

    private const int MaxLevel = 3;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Data = SaveSystem.Load();
    }

    public void CompleteLevel(int levelNumber, int score)
    {
        if (Data == null)
            Data = new SaveData();

        Data.bestScore = Mathf.Max(Data.bestScore, score);

        if (levelNumber < MaxLevel &&
            !Data.unlockedLevels.Contains(levelNumber + 1))
        {
            Data.unlockedLevels.Add(levelNumber + 1);
        }

        SaveSystem.Save(Data);
    }

    public void AddCoins(int amount)
    {
        Data.totalCoins += amount;
        SaveSystem.Save(Data);
    }

    public void SetMusicVolume(float volume)
    {
        Data.musicVolume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();

        SaveSystem.Save(Data);
    }

    public void SetSfxVolume(float volume)
    {
        Data.sfxVolume = volume;
        PlayerPrefs.SetFloat("sfxVolume", volume);
        PlayerPrefs.Save();

        SaveSystem.Save(Data);
    }

    public void ResetToDefaults()
    {
        SaveSystem.Delete();

        PlayerPrefs.DeleteKey("musicVolume");
        PlayerPrefs.DeleteKey("sfxVolume");
        PlayerPrefs.Save();

        Data = new SaveData();

        SaveSystem.Save(Data);
    }

    void OnApplicationPause(bool paused)
    {
        if (paused && Data != null)
            SaveSystem.Save(Data);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus && Data != null)
            SaveSystem.Save(Data);
    }
}
