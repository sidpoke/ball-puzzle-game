using UnityEngine;

public class GameMode_Zen : GameManager
{
    private ZenUI ui;

    protected override void Awake()
    {
        base.Awake();
        LevelManager.SetShowScores(false);
        ui = GetComponent<ZenUI>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //subscribe to events
        GameService.Instance.eventManager.BallSpecialTriggered += OnBallSpecialEvent;
        ui.PauseGame += OnPauseGame;
        ui.SwitchScene += OnSwitchScene;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        //unsubscribe events
        ui.PauseGame -= OnPauseGame;
        ui.SwitchScene -= OnSwitchScene;
        GameService.Instance.eventManager.BallSpecialTriggered -= OnBallSpecialEvent;
    }

    protected override void Start()
    {
        base.Start();
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
        LevelManager.FillAllPipes();
        LevelManager.FillPipe(LevelManager.LoaderPipe);
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
