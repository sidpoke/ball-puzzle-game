using TMPro;
using UnityEngine;

/// <summary>
/// Simple button class used to set an adventure mode level and load it
/// </summary>
public class Button_SelectAdventure : MonoBehaviour
{
    [Header("Button Setup")]
    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private TMP_Text completedText;
    [Header("Debug")]
    [SerializeField] private int _level;

    /// <summary>
    /// Called when the button is spawned, sets its text & level index to the provided values
    /// </summary>
    public void SetLevel(int level, string levelName)
    {
        _level = level;
        levelNameText.SetText(levelName);
    }

    /// <summary>
    /// Sets the current adventure manager level and loads the adventure mode scene
    /// </summary>
    public void LoadAdventure(string adventureSceneName)
    {
        GameService.Instance.audioManager.PlaySound("UIButton");
        GameService.Instance.adventure.SetLevel(_level);
        GameService.Instance.scenes.LoadScene(adventureSceneName);
    }
}
