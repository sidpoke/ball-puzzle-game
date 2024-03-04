using UnityEngine;

/// <summary>
/// GAME MANAGER CLASS - base class for any game mode.
/// Does not contain ui, timers, etc. but contains base overrides for a child gamemode class.
/// </summary>
public class GameManager : MonoBehaviour
{
    protected LevelManager _levelManager;

    protected bool gameOver = false;

    public LevelManager LevelManager { get { return _levelManager; } }

    protected virtual void Awake()
    {
        _levelManager = GetComponentInChildren<LevelManager>();
    }

    protected virtual void Start()
    {
        OnStartGame();
    }

    protected virtual void OnEnable() //subscribe to events
    {
        LevelManager.LoaderPipeFull += OnLoaderPipeFull;
        GameService.Instance.eventManager.BallSpecialTriggered += OnBallSpecialEvent;
        LevelManager.LevelBallSwitched += OnBallSwitched;
    }
    protected virtual void OnDisable() //unsubscribe events
    {
        LevelManager.LoaderPipeFull -= OnLoaderPipeFull;
        GameService.Instance.eventManager.BallSpecialTriggered -= OnBallSpecialEvent;
        LevelManager.LevelBallSwitched -= OnBallSwitched;
    }

    protected virtual void OnSwitchScene(string scene) //called from UI, Used to reset the scene too
    {
        Time.timeScale = 1;
        LevelManager.SetCanTouch(true);
        GameService.Instance?.scenes.LoadScene(scene);
    }

    protected virtual void OnStartGame()
    {
        gameOver = false;
        Time.timeScale = 1;
        LevelManager.SetCanTouch(true);
    }

    protected virtual void OnPauseGame(bool pause)
    {
        if (pause)
        {
            LevelManager.SetCanTouch(false);
            Time.timeScale = 0;
        }
        else
        {
            LevelManager.SetCanTouch(true);
            Time.timeScale = 1;
        }
    }
    protected virtual void OnGameOver() 
    {
        gameOver = true;
        Time.timeScale = 0;
        LevelManager.SetCanTouch(false);
    }

    protected virtual void OnLoaderPipeFull() { }
    protected virtual void OnBallSpecialEvent(BallController ball, BallSpecialEvent ballSpecialEvent) { }
    protected virtual void OnBallSwitched(BallController ballA, BallController ballB) { }
}
