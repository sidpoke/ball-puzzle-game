using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_SelectAdventure : MonoBehaviour
{
    [Header("Button Setup")]
    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private TMP_Text completedText;
    [Header("Debug")]
    [SerializeField] private int _level;

    /// <summary>
    /// Called when the button is spawned, calls a level
    /// </summary>
    public void SetLevel(int level, string levelName)
    {
        _level = level;
        levelNameText.SetText(levelName);
    }

    /// <summary>
    /// Sets the current adventure level and loads the adventure mode scene
    /// </summary>
    public void LoadAdventure(string adventureSceneName)
    {
        GameService.Instance.adventure.SetLevel(_level);
        GameService.Instance.scenes.LoadScene(adventureSceneName);
    }
}
