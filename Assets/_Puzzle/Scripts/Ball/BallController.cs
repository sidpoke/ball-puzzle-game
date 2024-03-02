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
public class BallController : MonoBehaviour
{
    protected IBallEventHandler eventHandler;
    protected IBallMovementController _movementController;
    protected IBallTouchInputProvider touchInputProvider;
    protected IBallAnimationController animationController;
    protected IBallAudioController audioController;
    protected IBallEffectsController effectsController;

    [Header("Ball Setup")]
    [SerializeField] protected BallColor _color;
    [SerializeField] protected BallSpecialEvent specialEvent; //NOT triggered by base class
    [SerializeField] protected int clearPoints = 100;
    [SerializeField] protected int ballDespawnTime = 2;

    [Header("Debug")]
    [SerializeField] protected PipeController _pipe;
    [SerializeField] protected int _pipeIndex = 0;
    [SerializeField] protected bool _explode = false;

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
        audioController = GetComponent<IBallAudioController>();
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

    public void SetPipe(PipeController pipe)
    {
        _pipe = pipe;
        OnBallPipeChanged();
    }

    public void SetPipeIndex(int index, bool move)
    {
        _pipeIndex = index;
        if (move && _pipe)
        {
            _movementController.Move(_pipe.WaypointProvider.Waypoints[_pipeIndex]);
        }
    }
    public void SetExplode(bool state)
    {
        _explode = state;
    }

    public void TriggerSpecialEvent()
    {
        GameService.Instance.eventManager.Event_BallSpecialEvent(this, specialEvent);
    }

    public void DestroyBall() //gives time to spare before destruction
    {
        OnBallDestroyed();
        Destroy(gameObject, ballDespawnTime);
    }

    public void ExplodeBall() //immediately destroys ball
    {
        OnBallExploded();
        Destroy(gameObject);
    }

    protected bool ColorMatchingPipe()
    {
        if (Pipe is SwitcherPipe pipe && PipeIndex == 0)
        {
            return ((int)BallColor == (int)pipe.PipeColor || BallColor == BallColor.Any);
        }
        return false;
    }

    //Override methods for child class
    protected virtual void OnBallPipeChanged(){}

    protected virtual void OnBallDestroyed()
    {
        eventHandler.BallScoreAdded(clearPoints);
        _movementController.FreeFall();
    }

    protected virtual void OnBallExploded()
    {
        eventHandler.BallScoreAdded(clearPoints);
    }

    protected virtual void OnBallTouched()
    {
        eventHandler.BallTouched(this);
    }

    protected virtual void OnBallSelected(BallController ball) 
    { 
        if(_pipe is LoaderPipe) { return; }

        effectsController.SetHighlight((ball != null && ball == this), (ball != null && ball.PipeIndex == _pipeIndex));
    }

    protected virtual void OnBallMovementFinished() 
    {
        if(ColorMatchingPipe())
        {
            Pipe.PipeStorage.Release();
        }
    }
}
