using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to store levels and to set the current level that needs to be loaded in adventure mode.
/// </summary>
public class AdventureManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private List<LevelObject> _levels;
    [Header("Debug")]
    [SerializeField] private int currentLevel;

    public LevelObject CurrentLevel { get { return _levels[currentLevel]; } }
    public int CurrentLevelIndex { get { return currentLevel; } }
    public List<LevelObject> Levels { get { return _levels; } }

    /// <summary>
    /// Set level to be loaded by the adventure game mode in runtime
    /// </summary>
    public void SetLevel(int level) // needed for arcade game mode
    {
        currentLevel = level;
    }

    //Future extensions could be saving a levels progress & highscore for replayability
    //public void SaveLevel(string level, int score)
    //{
    //    GameService.Instance.saveManager.SaveInt($"Level_{level}", score);
    //}

    //public int GetLevelScore(string level)
    //{
    //    return GameService.Instance.saveManager.LoadInt($"Level_{level}");
    //}
}
