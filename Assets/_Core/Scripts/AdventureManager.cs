using System.Collections.Generic;
using UnityEngine;

public class AdventureManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private List<LevelObject> _levels;
    [Header("Debug")]
    [SerializeField] private int currentLevel;

    public LevelObject CurrentLevel { get { return _levels[currentLevel]; } }
    public int CurrentLevelIndex { get { return currentLevel; } }
    public List<LevelObject> Levels { get { return _levels; } }

    public void SetLevel(int level) // needed for arcade game mode
    {
        currentLevel = level;
    }

    //public void SaveLevel(string level, int score)
    //{
    //    GameService.Instance.saveManager.SaveInt($"Level_{level}", score);
    //}

    //public int GetLevelScore(string level)
    //{
    //    return GameService.Instance.saveManager.LoadInt($"Level_{level}");
    //}
}
