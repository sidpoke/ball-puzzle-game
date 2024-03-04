using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

/// <summary>
/// A Combo object for the score manager, includes a prefab and how many balls in a row you need to trigger it
/// </summary>
[System.Serializable]
public class Combo
{
    public int count;
    public GameObject comboVfxPrefab;
}

public class ScoreManager : MonoBehaviour, IScoreManager
{
    public event Action<int> ScoreChanged;

    [Header("Combo Setup")]
    [SerializeField] private List<Combo> combos;
    [SerializeField] private float comboTime;
    [SerializeField] private Transform comboVFXSpawn;
    [SerializeField] private GameObject freezeTextPrefab; //in case of a freeze
    [SerializeField] private GameObject slowTextPrefab; //in case of a slow

    [Header("Debug")]
    [SerializeField] private int _score;
    [SerializeField] private int comboCount = 0;
    [SerializeField] private GameObject lastComboVfx;

    public int Score { get { return _score; } }

    private IEnumerator comboTimer;

    private void OnEnable() //subscribe to eventss
    {
        GameService.Instance.eventManager.BallScoreAdded += AddScore;
        GameService.Instance.eventManager.BallSpecialTriggered += OnBallSpecial;
    }

    private void OnDisable() //unsubscribe events
    {
        GameService.Instance.eventManager.BallScoreAdded -= AddScore;
        GameService.Instance.eventManager.BallSpecialTriggered -= OnBallSpecial;
    }

    /// <summary>
    /// When a special ball effect has been triggered, spawn the appropriate vfx for it
    /// </summary>
    public void OnBallSpecial(BallController ball, BallSpecialEvent ballEvent)
    {
        switch (ballEvent)
        {
            case BallSpecialEvent.Slow:
                lastComboVfx = Instantiate(slowTextPrefab, comboVFXSpawn.position, Quaternion.identity) as GameObject;
                break;
            case BallSpecialEvent.Freeze:
                lastComboVfx = Instantiate(freezeTextPrefab, comboVFXSpawn.position, Quaternion.identity) as GameObject;
                break;
        }
    }

    /// <summary>
    /// Add points to the score counter
    /// </summary>
    public void AddScore(int points)
    {
        _score += points;
        ScoreChanged?.Invoke(Score);
        ComboAdd();
    }
    
    /// <summary>
    /// Reset score
    /// </summary>
    public void ResetScore()
    {
        _score = 0;
        ScoreChanged?.Invoke(Score);
    }

    /// <summary>
    /// Add an increment to the combo
    /// </summary>
    private void ComboAdd()
    {
        if (comboTimer != null) //if combo timer running, kill the current timer
        {
            StopCoroutine(comboTimer); 
            comboTimer = null;
        }

        comboTimer = ScoreComboTimer(comboTime); //start a timer for the combo
        StartCoroutine(comboTimer);
        comboCount++;
    }

    /// <summary>
    /// Called when the combo timer runs out
    /// </summary>
    private void ComboEnd()
    {
        if (lastComboVfx != null) { Destroy(lastComboVfx.gameObject); }
        if(comboCount + 1 > combos[0].count)
        {
            GameObject comboPrefab = combos.Where(combo => combo.count <= comboCount + 1).Last().comboVfxPrefab;
            lastComboVfx = Instantiate(comboPrefab, comboVFXSpawn.position, Quaternion.identity) as GameObject;
        }

        comboCount = 0;
    }

    /// <summary>
    /// Called repeatedly until it can run out
    /// </summary>
    private IEnumerator ScoreComboTimer(float timeOut)
    {
        yield return new WaitForSeconds(timeOut);
        ComboEnd();
        comboTimer = null;
    }
}