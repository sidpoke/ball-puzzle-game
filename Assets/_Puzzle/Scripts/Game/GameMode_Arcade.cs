using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMode_Arcade : GameManager
{
    private ITimerProvider timer;
    private IScoreManager scoreManager;

    [SerializeField]
    private TMP_Text scoreText;
    
    protected override void Awake()
    {
        base.Awake();
        timer = GetComponent<ITimerProvider>();
        scoreManager = GetComponent<IScoreManager>();
        timer.Timeout += OnTimerTimeout;
    }

    protected override void Start()
    {
        base.Start();
        LevelManager.FillAllPipes();
        timer.TimerStart();
        LevelManager.Pipes.ForEach(pipe => pipe.PipeStorage.BallRemoved += OnPipeRelease);
    }

    // Update is called once per frame
    private void Update()
    {
        //Cheats
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

    public void OnPipeRelease(BallController ball)
    {
        //add score
        scoreManager.AddScore(100); //stupid stupid stupid magic numbers CHANGE IT
        scoreText.SetText($"Score: {scoreManager.Score.ToString()}");
    }

    private void OnTimerTimeout()
    {
        LevelManager.AddBallToLoader();
    }
}
