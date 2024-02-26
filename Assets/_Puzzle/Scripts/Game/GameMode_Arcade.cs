using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode_Arcade : GameManager
{
    private ITimerProvider _timer;
    
    protected override void Awake()
    {
        base.Awake();
        _timer = GetComponent<ITimerProvider>();
        _timer.Timeout += OnTimerTimeout;
    }

    protected override void Start()
    {
        base.Start();
        LevelManager.FillAllPipes();
        _timer.TimerStart();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LevelManager.AddBallToLoader();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            LevelManager.ReleaseLoaderBall();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            LevelManager.KillRandomBall();
        }
    }
    private void OnTimerTimeout()
    {
        LevelManager.AddBallToLoader();
    }
}
