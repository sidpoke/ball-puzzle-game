using System.Collections;
using UnityEngine;

public class GameMode_Adventure : GameManager
{
    private IScoreManager scoreManager;
    private AdventureUI ui;

    [Header("Debug")]
    [SerializeField] private float endGameWaitTime = 2.0f;
    [SerializeField] private LevelObject currentLevel;
    [SerializeField] private int ballSwitchCount = 0;
    [SerializeField] private bool gameOver = false;

    protected override void Awake()
    {
        base.Awake();
        LevelManager.SetShowScores(true);
        scoreManager = GetComponent<IScoreManager>();
        ui = GetComponent<AdventureUI>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //subscribe to events
        scoreManager.ScoreChanged += OnScoreChanged;
        ui.PauseGame += OnPauseGame;
        ui.SwitchScene += OnSwitchScene;
        GameService.Instance.eventManager.BallSpecialTriggered += OnBallSpecialEvent;
        LevelManager.LevelBallSwitched += OnBallSwitched;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        //unsubscribe events
        scoreManager.ScoreChanged -= OnScoreChanged;
        ui.PauseGame -= OnPauseGame;
        ui.SwitchScene -= OnSwitchScene;
        GameService.Instance.eventManager.BallSpecialTriggered -= OnBallSpecialEvent;
        LevelManager.LevelBallSwitched -= OnBallSwitched;
    }

    protected override void Start()
    {
        base.Start();
        currentLevel = GameService.Instance.adventure.CurrentLevel;
        ui.SetBallSwapTriesText(currentLevel.AmountOfSwitches - ballSwitchCount);
        StartGame();
    }

    public void Update()
    {
        if (!LevelManager.LoaderPipe.PipeStorage.IsFull)
        {
            LevelManager.AddBallToPipe(LevelManager.LoaderPipe, false);
        }
    }

    private void StartGame()
    {
        LevelManager.SetCanTouch(true);
        LevelManager.FillAllPipes(currentLevel);
        OnScoreChanged(scoreManager.Score);
    }

    private void GameOver()
    {
        gameOver = true;

        //stop game
        Time.timeScale = 0;
        LevelManager.SetCanTouch(false);

        //open panel
        ui.OpenGameOverMenu(scoreManager.Score, currentLevel.ScoreToBeat);
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
            LevelManager.SetCanTouch(false);
            Time.timeScale = 0;
            ui.OpenPauseMenu();
        }
        else
        {
            ui.ClosePauseMenu();
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
        ui.SetScoreText(score, currentLevel.ScoreToBeat);

        if(scoreManager.Score >= currentLevel.ScoreToBeat)
        {
            GameOver();
        }
    }

    public void OnBallSwitched(BallController ballA, BallController ballB)
    {
        ballSwitchCount++;

        ui.SetBallSwapTriesText(currentLevel.AmountOfSwitches - ballSwitchCount);

        if(ballSwitchCount >= currentLevel.AmountOfSwitches)
        {
            LevelManager.SetCanTouch(false);
            StartCoroutine(WaitForFinalScore(endGameWaitTime)); //succeeded maximum amount of ball swaps
        }
    }

    IEnumerator WaitForFinalScore(float time)
    {
        yield return new WaitForSeconds(time);
        GameOver();
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
                break;
            case BallSpecialEvent.Freeze:
                break;
            case BallSpecialEvent.Bomb:
                LevelManager.DestroyBallsNineBlock(ball);
                break;
            case BallSpecialEvent.Laser:
                LevelManager.DestroyBallsLine(ball);
                break;
        }
    }
}
