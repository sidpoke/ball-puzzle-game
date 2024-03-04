using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Used to decide whether or not a ball can go through a specific pipe
/// </summary>
public enum BallColor
{
    Red = 0,
    Green = 1,
    Blue = 2,
    Yellow = 3,
    Any = 4,
    None = 5
}

/// <summary>
/// Used to send information towards the level manager whenever a special ball event needs to be triggered
/// </summary>
public enum BallSpecialEvent
{
    None,
    Slow,
    Freeze,
    Bomb,
    Laser
}

/// <summary>
/// BallController couples all sub-components of the ball into a controller class with callable and overridable methods.
/// </summary>
public class BallController : MonoBehaviour
{
    protected IBallEventHandler eventHandler;
    protected IBallMovementController _movementController;
    protected IBallTouchInputProvider touchInputProvider;
    protected IBallAnimationController animationController;
    protected IBallAudioHandler audioController;
    protected IBallEffectsController effectsController;

    [Header("Ball Setup")]
    [SerializeField] protected BallColor _color;
    [SerializeField] protected BallSpecialEvent specialEvent; //NOT triggered by base class
    [SerializeField] protected int clearPoints = 100;
    [SerializeField] protected Color clearVFXColor = Color.white;
    [SerializeField] protected int ballDespawnTime = 2;

    [Header("Debug")]
    [SerializeField] protected PipeController _pipe;
    [SerializeField] protected int _pipeIndex = 0;
    [SerializeField] protected bool _explode = false;
    [SerializeField] protected bool canPlayDropSound = false;
    [SerializeField] protected bool showScore = true;

    public BallColor BallColor { get { return _color; } }
    public PipeController Pipe { get { return _pipe; } }
    public int PipeIndex { get { return _pipeIndex; } }
    public IBallMovementController MovementController { get { return _movementController; }}
    public bool Explode { get { return _explode; } }

    protected virtual void Awake()
    {
        eventHandler = GetComponent<IBallEventHandler>();
        _movementController = GetComponent<IBallMovementController>();
        touchInputProvider = GetComponent<IBallTouchInputProvider>();
        animationController = GetComponent<IBallAnimationController>();
        audioController = GetComponent<IBallAudioHandler>();
        effectsController = GetComponent<IBallEffectsController>();
    }

    protected virtual void OnEnable()
    {
        //subscribe to events
        GameService.Instance.eventManager.LevelBallSelected += OnBallSelected;
        touchInputProvider.BallTouched += OnBallTouched;
        _movementController.FinishedMoving += OnBallMovementFinished;
    }

    protected virtual void OnDisable()
    {
        //unsubscribe events
        GameService.Instance.eventManager.LevelBallSelected -= OnBallSelected;
        touchInputProvider.BallTouched -= OnBallTouched;
        _movementController.FinishedMoving -= OnBallMovementFinished;
    }

    /// <summary>
    /// Sets the pipe this ball controller is inside of
    /// </summary>
    public void SetPipe(PipeController pipe)
    {
        if(_pipe != null || pipe is LoaderPipe) { canPlayDropSound = true; }
        _pipe = pipe;
        OnBallPipeChanged();
    }

    /// <summary>
    /// Sets the pipe index ("at which position this ball is inside of a pipe")
    /// "move" simply sets wheter or not the ball should also move to the new position once set
    /// </summary>
    public void SetPipeIndex(int index, bool move)
    {
        _pipeIndex = index;
        if (move && _pipe)
        {
            _movementController.Move(_pipe.WaypointProvider.Waypoints[_pipeIndex]);
        }
        if(index == 0) //for resetting the drop sound (avoid spamming)
        {
            canPlayDropSound = true;
        }
    }

    /// <summary>
    /// Sets whether or not the ball should explode when destroyed
    /// </summary>
    public void SetExplode(bool state)
    {
        _explode = state;
    }

    /// <summary>
    /// Sets whether or not the ball should spawn a score vfx when destroyed
    /// </summary>
    public void SetShowScore(bool state)
    {
        showScore = state;
    }

    /// <summary>
    /// Triggers a special event (eg. freeze, slow, bomb, laser)
    /// </summary>
    protected void TriggerSpecialEvent() //triggers a special event globally
    {
        GameService.Instance.eventManager.Event_BallSpecialEvent(this, specialEvent);
    }

    /// <summary>
    /// Destroys the ball but gives time to spare before destruction, calls OnBallDestroyed
    /// </summary>
    public void DestroyBall()
    {
        OnBallDestroyed();
        Destroy(gameObject, ballDespawnTime);
    }

    /// <summary>
    /// Destroys the ball immediately, calls OnBallExploded
    /// </summary>
    public void ExplodeBall()
    {
        OnBallExploded();
        Destroy(gameObject);
    }

    /// <summary>
    /// Checks whether or not this ball matches with the pipes color
    /// </summary>
    protected bool ColorMatchingPipe()
    {
        if (Pipe is SwitcherPipe pipe && PipeIndex == 0)
        {
            return ((int)BallColor == (int)pipe.PipeColor || BallColor == BallColor.Any);
        }
        return false;
    }

    //Override methods for child class
    protected virtual void OnBallPipeChanged(){} //called from SetPipe method

    protected virtual void OnBallDestroyed() //called from public DestroyBall method
    {
        eventHandler.BallScoreAdded(clearPoints);
        _movementController.FreeFall();
        if(showScore)
        {
            effectsController.SpawnScoreText(Pipe.WaypointProvider.ScoreVFXSpawnPoint, clearPoints, clearVFXColor);
        }
    }

    protected virtual void OnBallExploded() //called from public ExplodeBall method
    {
        eventHandler.BallScoreAdded(clearPoints);
        if (showScore)
        {
            effectsController.SpawnScoreText((Vector2)transform.position, clearPoints, clearVFXColor);
        }
        effectsController.SpawnExplosion((Vector2)transform.position);
    }

    protected virtual void OnBallTouched() //called by touch provider class
    {
        if(Pipe is SwitcherPipe)
        {
            eventHandler.BallTouched(this);
            audioController.PlayAudio("BallClick");
        }
    }

    protected virtual void OnBallSelected(BallController ball) //called by global event manager
    { 
        if(_pipe is LoaderPipe) { return; }

        effectsController.SetHighlight((ball != null && ball == this), (ball != null && ball.PipeIndex == _pipeIndex));
    }

    protected virtual void OnBallMovementFinished() //called by movement controller
    {
        if(ColorMatchingPipe())
        {
            Pipe.PipeStorage.Release();
            audioController.PlayAudio("BallRelease");
        }
        else if(canPlayDropSound)
        {
            audioController.PlayAudio("BallDrop");
            canPlayDropSound = false;
        }
    }
}
