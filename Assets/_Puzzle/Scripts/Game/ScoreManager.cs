using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour, IScoreManager
{
    public event Action<int> ScoreChanged;


    private int _score;
    public int Score { get { return _score; } }

    private void Awake()
    {
        GameService.Instance.eventManager.BallScoreAdded += AddScore;
    }

    //extend with combos?
    public void AddScore(int points)
    {
        _score += points;
        ScoreChanged?.Invoke(Score);
    }
}
