using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// A struct to define an arcade mode difficulty. 
/// ScoreTrigger is the value needed to reach this difficulty stage.
/// </summary>
[System.Serializable]
public struct ArcadeDifficulty
{
    public string DifficultyName;
    public float ScoreTrigger;
    public float DifficultyBallTimer;
}

/// <summary>
/// ARCADE GAME MODE - The LoaderPipe will get new balls every x seconds, if it's full then the game is over
/// </summary>
public class GameMode_Arcade : GameManager
{
    private ITimerProvider timer; //timer needed for spawning balls
    private IScoreManager scoreManager;
    private ArcadeUI ui;

    [Header("Arcade Settings")]
    [SerializeField] private List<ArcadeDifficulty> difficulties = new List<ArcadeDifficulty>();
    [SerializeField] private int startDifficulty = 0;

    [Header("Special Ball Effects")]
    [SerializeField] private float slowTime = 6.0f;
    [SerializeField] private float slowRatio = 2.0f;
    [SerializeField] private float freezeTime = 3.0f;

    [Header("Debug")]
    [SerializeField] private int currentDifficulty = 0;

    private IEnumerator freezeTimer; //Coroutine for freeze ball
    private IEnumerator slowTimer; //Coroutine for slow ball

    protected override void Awake() //Get components
    {
        base.Awake();
        timer = GetComponent<ITimerProvider>();
        scoreManager = GetComponent<IScoreManager>();
        ui = GetComponent<ArcadeUI>();
    }

    protected override void OnEnable() //subscribe to events
    {
        base.OnEnable();
        scoreManager.ScoreChanged += OnScoreChanged;
        timer.Timeout += OnTimerTimeout;
        ui.PauseGame += OnPauseGame;
        ui.SwitchScene += OnSwitchScene;
    }

    protected override void OnDisable() //unsubscribe events
    {
        base.OnDisable();
        scoreManager.ScoreChanged -= OnScoreChanged;
        timer.Timeout -= OnTimerTimeout;
        ui.PauseGame -= OnPauseGame;
        ui.SwitchScene -= OnSwitchScene;
    }

    //private void Update()
    //{
    //    //Cheats
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        LevelManager.AddBallToPipe(LevelManager.LoaderPipe, true);
    //    }
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        LevelManager.LoaderPipe.PipeStorage.Release();
    //    }
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        int random = Random.Range(0, LevelManager.LoaderPipe.PipeStorage.Balls.Count);
    //        LevelManager.LoaderPipe.PipeStorage.RemoveAt(random);
    //    }
    //}

    private void SetDifficulty(int difficulty) //change timer to a new timeout
    {
        currentDifficulty = difficulty;
        ui.SetDifficultyText(difficulties[currentDifficulty].DifficultyName);
        timer.SetTimerTime(difficulties[currentDifficulty].DifficultyBallTimer);
    }

    protected override void OnStartGame()
    {
        base.OnStartGame();

        LevelManager.SetShowScores(true);
        LevelManager.FillAllPipes();
        ui.SetScoreText(0);
        timer.TimerStart();
        SetDifficulty(startDifficulty);
    }

    protected override void OnGameOver()
    {
        base.OnGameOver();

        //highscores
        int highScore = GameService.Instance.saveManager.LoadInt("HighScore");
        GameService.Instance.saveManager.SaveInt("LastScore", scoreManager.Score);
        if (scoreManager.Score > highScore) { GameService.Instance.saveManager.SaveInt("HighScore", scoreManager.Score); }

        //open game over panel
        ui.OpenGameOverMenu(scoreManager.Score, scoreManager.Score > highScore ? scoreManager.Score : highScore);
    }

    protected override void OnPauseGame(bool pause) //called from UI
    {
        base.OnPauseGame(pause);
        if (pause)
        {
            timer.TimerStop();
            ui.OpenPauseMenu();
        }
        else
        {
            ui.ClosePauseMenu();
            timer.TimerStart();
        }
    }

    protected override void OnLoaderPipeFull() //called from LevelManager
    {
        base.OnLoaderPipeFull();
        OnGameOver();
    }

    private void OnScoreChanged(int score) //Updating difficulty & adjusting score text
    {
        if (difficulties != null && difficulties.Count > (currentDifficulty + 1))
        {
            if(score >= difficulties[currentDifficulty + 1].ScoreTrigger)
            {
                SetDifficulty(currentDifficulty + 1);
            }
        }

        ui.SetScoreText(score);
    }

    private void OnTimerTimeout() //Add a ball when the timer reaches its end
    {
        LevelManager.AddBallToPipe(LevelManager.LoaderPipe, true);
    }

    /// <summary>
    /// Used to trigger special events like bombs, freeze, etc.
    /// </summary>
    /// <param name="ball">The ball that invoked the request (possibly null!!)</param>
    protected override void OnBallSpecialEvent(BallController ball, BallSpecialEvent specialEvent)
    {
        switch (specialEvent)
        {
            case BallSpecialEvent.Slow:
                //slow timer resets when another slow ball releasese
                if (slowTimer != null){ 
                    StopCoroutine(slowTimer);
                    timer.SetTimerTime(difficulties[currentDifficulty].DifficultyBallTimer);
                }
                slowTimer = StartSlowTimer(slowTime, slowRatio);
                StartCoroutine(slowTimer);
                break;
            case BallSpecialEvent.Freeze:
                //freeze timer resets when another freeze ball releasese
                if (freezeTimer != null) { StopCoroutine(freezeTimer); }
                freezeTimer = StartFreezeTimer(freezeTime);
                StartCoroutine(freezeTimer);
                break;
            case BallSpecialEvent.Bomb:
                LevelManager.DestroyBallsNineBlock(ball);
                break;
            case BallSpecialEvent.Laser:
                LevelManager.DestroyBallsLine(ball);
                break;
        }
    }

    /// <summary>
    /// Slow Timer Coroutine used for a slow ball. 
    /// Changes timer speed to current difficulty speed * amount for x seconds.
    /// </summary>
    private IEnumerator StartSlowTimer(float time, float amount)
    {
        if (amount == 0) { slowTimer = null; yield break; }
        timer.SetTimerTime(difficulties[currentDifficulty].DifficultyBallTimer * amount);
        yield return new WaitForSeconds(time);
        timer.SetTimerTime(difficulties[currentDifficulty].DifficultyBallTimer);
        slowTimer = null;
    }

    /// <summary>
    /// Freeze Timer Coroutine used for a freeze ball. 
    /// Stops and resets timer for x seconds.
    /// </summary>
    private IEnumerator StartFreezeTimer(float time)
    {
        if (gameOver) { freezeTimer = null; yield break; } // don't trigger when game just ended
        timer.TimerStop();
        timer.TimerReset();
        yield return new WaitForSeconds(time);
        if (gameOver) { freezeTimer = null; yield break; } // don't trigger when game ended after freeze
        timer.TimerStart();
        freezeTimer = null;
    }
}
