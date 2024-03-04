using System.Collections;
using UnityEngine;

/// <summary>
/// ADVENTURE GAME MODE - Loads a predefined level, you need to beat a score x within n moves to win
/// Should n moves <= 0 the player has lost
/// </summary>
public class GameMode_Adventure : GameManager
{
    private IScoreManager scoreManager;
    private AdventureUI ui;

    [Header("Debug")]
    [SerializeField] private float endGameWaitTime = 2.0f;
    [SerializeField] private LevelObject currentLevel;
    [SerializeField] private int ballSwitchCount = 0;

    protected override void Awake()
    {
        base.Awake();
        scoreManager = GetComponent<IScoreManager>();
        ui = GetComponent<AdventureUI>();
    }

    protected override void OnEnable() //subscribe to events
    {
        base.OnEnable();
        scoreManager.ScoreChanged += OnScoreChanged;
        ui.PauseGame += OnPauseGame;
        ui.SwitchScene += OnSwitchScene;
    }

    protected override void OnDisable() //unsubscribe events
    {
        base.OnDisable();
        scoreManager.ScoreChanged -= OnScoreChanged;
        ui.PauseGame -= OnPauseGame;
        ui.SwitchScene -= OnSwitchScene;
    }

    private void Update()
    {
        if (!LevelManager.LoaderPipe.PipeStorage.IsFull) //simply fill the loader pipe as long as it's not full
        {
            LevelManager.AddBallToPipe(LevelManager.LoaderPipe, false);
        }
    }

    protected override void OnStartGame()
    {
        base.OnStartGame();

        LevelManager.SetShowScores(true);
        currentLevel = GameService.Instance.adventure.CurrentLevel; //fetch level
        ui.SetBallSwapTriesText(currentLevel.AmountOfSwitches - ballSwitchCount);
        LevelManager.FillAllPipes(currentLevel);
        OnScoreChanged(scoreManager.Score); //set score in the beginning
    }

    protected override void OnGameOver()
    {
        base.OnGameOver();

        //open panel
        ui.OpenGameOverMenu(scoreManager.Score, currentLevel.ScoreToBeat);
    }

    protected override void OnPauseGame(bool pause) //called from UI
    {
        base.OnPauseGame(pause);
        if (pause){ ui.OpenPauseMenu(); }
        else { ui.ClosePauseMenu(); }
    }

    private void OnScoreChanged(int score)
    {
        ui.SetScoreText(score, currentLevel.ScoreToBeat);

        if(scoreManager.Score >= currentLevel.ScoreToBeat) //end game if score limit is reached
        {
            OnGameOver();
        }
    }

    protected override void OnBallSwitched(BallController ballA, BallController ballB)
    {
        ballSwitchCount++;

        ui.SetBallSwapTriesText(currentLevel.AmountOfSwitches - ballSwitchCount);

        if(ballSwitchCount >= currentLevel.AmountOfSwitches) //end game if reached maximum amount of ball swaps
        {
            LevelManager.SetCanTouch(false);
            StartCoroutine(WaitForFinalScore(endGameWaitTime)); //wait for the last ball to finish moving
        }
    }

    IEnumerator WaitForFinalScore(float time)
    {
        yield return new WaitForSeconds(time);
        OnGameOver();
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
