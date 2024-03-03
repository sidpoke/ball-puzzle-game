using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

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

    [Header("Debug")]
    [SerializeField] private int _score;
    [SerializeField] private int comboCount = 0;
    [SerializeField] private GameObject lastComboVfx;

    public int Score { get { return _score; } }

    private IEnumerator comboTimer;

    private void OnEnable()
    {
        //subscribe to events
        GameService.Instance.eventManager.BallScoreAdded += AddScore;
    }
    private void OnDisable()
    {
        //unsubscribe events
        GameService.Instance.eventManager.BallScoreAdded -= AddScore;
    }

    //extend with combos?


    public void AddScore(int points)
    {
        _score += points;
        ScoreChanged?.Invoke(Score);
        ComboAdd();
    }
    
    public void ResetScore()
    {
        _score = 0;
        ScoreChanged?.Invoke(Score);
    }

    private void ComboAdd()
    {
        if (comboTimer != null)
        {
            StopCoroutine(comboTimer); 
            comboTimer = null;
        }

        comboTimer = ScoreComboTimer(comboTime);
        StartCoroutine(comboTimer);
        comboCount++;
    }

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

    private IEnumerator ScoreComboTimer(float timeOut)
    {
        yield return new WaitForSeconds(timeOut);
        ComboEnd();
        comboTimer = null;
    }
}
