using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private int currentDifficulty = 0;

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
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        //unsubscribe events
        scoreManager.ScoreChanged -= OnScoreChanged;
        timer.Timeout -= OnTimerTimeout;
        ui.PauseGame -= OnPauseGame;
        ui.SwitchScene -= OnSwitchScene;
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
}
