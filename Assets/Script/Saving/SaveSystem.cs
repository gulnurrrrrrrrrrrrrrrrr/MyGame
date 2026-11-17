
using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string FilePath =>
        Path.Combine(
            Application.persistentDataPath,
            "save.json"
        );

    private static string TempPath =>
        Path.Combine(
            Application.persistentDataPath,
            "save.tmp"
        );

    public static void Save(SaveData data)
    {
        try
        {
            Directory.CreateDirectory(
                Application.persistentDataPath
            );

            string json = JsonUtility.ToJson(data, true);

            File.WriteAllText(TempPath, json);

            if (File.Exists(FilePath))
            {
                File.Replace(TempPath, FilePath, null);
            }
            else
            {
                File.Move(TempPath, FilePath);
            }

            Debug.Log("Game saved: " + FilePath);
            Debug.Log("SAVE FILE PATH: " + FilePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Save failed: " + e.Message);
        }
    }

    public static SaveData Load()
    {
        if (!File.Exists(FilePath))
        {
            Debug.Log("Save file not found. Using defaults.");
            return new SaveData();
        }

        try
        {
            string json = File.ReadAllText(FilePath);

            if (string.IsNullOrWhiteSpace(json))
                return new SaveData();

            SaveData data =
                JsonUtility.FromJson<SaveData>(json);

            if (data == null)
                return new SaveData();

            if (data.version != 1)
                return new SaveData();

            if (data.unlockedLevels == null ||
                data.unlockedLevels.Count == 0)
            {
                data.unlockedLevels =
                    new System.Collections.Generic.List<int> { 1 };
            }

            if (!data.unlockedLevels.Contains(1))
                data.unlockedLevels.Insert(0, 1);

            return data;
        }
        catch (Exception e)
        {
            Debug.LogWarning(
                "Broken save file. Using defaults: " + e.Message
            );

            return new SaveData();
        }
    }

    public static void Delete()
    {
        try
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);

            if (File.Exists(TempPath))
                File.Delete(TempPath);
        }
        catch (Exception e)
        {
            Debug.LogError("Delete failed: " + e.Message);
        }
    }
}
