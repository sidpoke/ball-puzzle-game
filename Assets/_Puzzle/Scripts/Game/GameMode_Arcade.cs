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
    //private Arcade_UIManager  :)))

    [Header("UI Setup")]
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]    
    private TMP_Text difficultyText;

    [Header("Arcade Settings")]
    [SerializeField]
    private List<ArcadeDifficulty> difficulties = new List<ArcadeDifficulty>();
    [SerializeField]
    private int startDifficulty = 0;
    [SerializeField]
    private int currentDifficulty = 0;

    protected override void Awake()
    {
        base.Awake();
        timer = GetComponent<ITimerProvider>();
        scoreManager = GetComponent<IScoreManager>();

        scoreManager.ScoreChanged += OnScoreChanged;
        timer.Timeout += OnTimerTimeout;
    }

    protected override void Start()
    {
        base.Start();
        LevelManager.FillAllPipes();
        timer.TimerStart();
        SetDifficulty(startDifficulty);
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

    private void SetDifficulty(int difficulty)
    {
        currentDifficulty = difficulty;

        difficultyText.SetText(difficulties[currentDifficulty].DifficultyName);
        timer.SetTimerTime(difficulties[currentDifficulty].DifficultyBallTimer);
    }

    protected override void OnLoaderPipeFull() //game over
    {
        base.OnLoaderPipeFull();
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

        scoreText.SetText($"Score: {score.ToString()}");
    }

    private void OnTimerTimeout()
    {
        LevelManager.AddBallToPipe(LevelManager.LoaderPipe);
    }
}
