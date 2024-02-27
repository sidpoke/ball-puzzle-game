using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour, IScoreManager
{
    private int _score;

    public int Score { get { return _score; } }

    public void AddScore(int points)
    {
        _score += points;
       //GameService.Instance.eventManager.EventScoreUpdate(_score);
    }
}
