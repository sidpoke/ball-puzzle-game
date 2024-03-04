using UnityEngine;

/// <summary>
/// ZEN GAME MODE - No timer, no scores. This infinite game mode is supposed to keep you busy and is used for practice.
/// </summary>
public class GameMode_Zen : GameManager
{
    private ZenUI ui;

    protected override void Awake()
    {
        base.Awake();
        LevelManager.SetShowScores(false);
        ui = GetComponent<ZenUI>();
    }

    protected override void OnEnable() //subscribe to events
    {
        base.OnEnable();
        ui.PauseGame += OnPauseGame;
        ui.SwitchScene += OnSwitchScene;
    }

    protected override void OnDisable() //unsubscribe events
    {
        base.OnDisable(); 
        ui.PauseGame -= OnPauseGame;
        ui.SwitchScene -= OnSwitchScene;
    }

    public void Update()
    {
        if (!LevelManager.LoaderPipe.PipeStorage.IsFull) //simply fill the loader pipe as long as it's not full
        {
            LevelManager.AddBallToPipe(LevelManager.LoaderPipe, false);
        }
    }

    protected override void OnStartGame()
    {
        base.OnStartGame();
        LevelManager.FillAllPipes();
    }

    protected override void OnPauseGame(bool pause) //called over UI
    {
        base.OnPauseGame(pause);
        if (pause) { ui.OpenPauseMenu(); }
        else { ui.ClosePauseMenu(); }
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
