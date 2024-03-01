using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores and saves information of the game progress.
/// </summary>
public class SaveManager : MonoBehaviour, ISaveManager
{
    public void SaveString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public string LoadString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public void Delete(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }
    public int LoadInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }
}
