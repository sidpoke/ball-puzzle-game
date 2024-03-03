using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public struct ArcadeDifficulty
{
    public string DifficultyName;
    public float ScoreTrigger;
    public float DifficultyBallTimer;
}

public class GameMode_Arcade : GameManager
{
    private ITimerProvider timer;
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
    [SerializeField] private bool gameOver = false;
    [SerializeField] private int currentDifficulty = 0;

    private IEnumerator freezeTimer;
    private IEnumerator slowTimer;

    protected override void Awake()
    {
        base.Awake();
        timer = GetComponent<ITimerProvider>();
        scoreManager = GetComponent<IScoreManager>();
        ui = GetComponent<ArcadeUI>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //subscribe to events
        scoreManager.ScoreChanged += OnScoreChanged;
        timer.Timeout += OnTimerTimeout;
        ui.PauseGame += OnPauseGame;
        ui.SwitchScene += OnSwitchScene;
        GameService.Instance.eventManager.BallSpecialTriggered += OnBallSpecialEvent;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        //unsubscribe events
        scoreManager.ScoreChanged -= OnScoreChanged;
        timer.Timeout -= OnTimerTimeout;
        ui.PauseGame -= OnPauseGame;
        ui.SwitchScene -= OnSwitchScene;
        GameService.Instance.eventManager.BallSpecialTriggered -= OnBallSpecialEvent;
    }

    protected override void Start()
    {
        base.Start();
        StartGame();
    }

    private void Update()
    {
        //Cheats
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LevelManager.AddBallToPipe(LevelManager.LoaderPipe);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            LevelManager.LoaderPipe.PipeStorage.Release();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            int random = Random.Range(0, LevelManager.LoaderPipe.PipeStorage.Balls.Count);
            LevelManager.LoaderPipe.PipeStorage.RemoveAt(random);
        }
    }

    private void StartGame()
    {
        LevelManager.SetCanTouch(true);
        LevelManager.FillAllPipes();
        timer.TimerStart();
        SetDifficulty(startDifficulty);
    }

    private void GameOver()
    {
        gameOver = true;

        //stop game
        Time.timeScale = 0;
        timer.TimerStop();
        LevelManager.SetCanTouch(false);
        
        //highscores
        int highScore = GameService.Instance.saveManager.LoadInt("HighScore");
        GameService.Instance.saveManager.SaveInt("LastScore", scoreManager.Score);
        if (scoreManager.Score > highScore) { GameService.Instance.saveManager.SaveInt("HighScore", scoreManager.Score); }

        //open panel
        ui.OpenGameOverMenu(scoreManager.Score, scoreManager.Score > highScore ? scoreManager.Score : highScore);
    }

    private void SetDifficulty(int difficulty)
    {
        currentDifficulty = difficulty;

        ui.SetDifficultyText(difficulties[currentDifficulty].DifficultyName);
        timer.SetTimerTime(difficulties[currentDifficulty].DifficultyBallTimer);
    }

    private void OnSwitchScene(string scene) //reset too
    {
        Time.timeScale = 1;
        LevelManager.SetCanTouch(true);
        GameService.Instance?.scenes.LoadScene(scene);
    }

    private void OnPauseGame(bool pause) //called from UI
    {
        if (pause)
        {
            timer.TimerStop();
            LevelManager.SetCanTouch(false);
            Time.timeScale = 0;
            ui.OpenPauseMenu();
        }
        else
        {
            ui.ClosePauseMenu();
            timer.TimerStart();
            LevelManager.SetCanTouch(true);
            Time.timeScale = 1;
        }
    }

    protected override void OnLoaderPipeFull()
    {
        base.OnLoaderPipeFull();
        GameOver();
    }

    private void OnScoreChanged(int score)
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

    private void OnTimerTimeout()
    {
        LevelManager.AddBallToPipe(LevelManager.LoaderPipe);
    }

    /// <summary>
    /// Used to trigger special events like bombs, freeze, etc.
    /// </summary>
    /// <param name="ball">The ball that invoked the request (might be null!!)</param>
    public void OnBallSpecialEvent(BallController ball, BallSpecialEvent specialEvent)
    {
        switch (specialEvent)
        {
            case BallSpecialEvent.Slow:
                //slow timer resets when another slow ball releasese
                if (slowTimer != null){ 
                    StopCoroutine(slowTimer);
                    timer.SetTimerTime(timer.TimerTime * (1 / slowRatio));
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

    private IEnumerator StartSlowTimer(float time, float amount)
    {
        if (amount == 0) { slowTimer = null; yield break; }
        timer.SetTimerTime(timer.TimerTime * amount);
        yield return new WaitForSeconds(time);
        timer.SetTimerTime(timer.TimerTime * (1 / amount));
        slowTimer = null;
    }

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
