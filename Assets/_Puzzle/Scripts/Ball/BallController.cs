using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    protected IBallEventHandler eventHandler;
    protected IBallMovementController _movementController;
    protected IBallTouchInputProvider touchInputProvider;
    protected BallEffectsHandler ballEffects;


    [Header("Ball Setup")]
    [SerializeField] private int clearPoints = 100;
    [SerializeField] private int ballDespawnTime = 2;

    [Header("Debug")]
    [SerializeField] private PipeController _pipe;
    [SerializeField] private int _pipeIndex = 0;

    public PipeController Pipe { get { return _pipe; } }
    public int PipeIndex { get { return _pipeIndex; } }
    public IBallMovementController MovementController { get { return _movementController; }}

    protected virtual void Awake()
    {
        eventHandler = GetComponent<IBallEventHandler>();
        _movementController = GetComponent<IBallMovementController>();
        touchInputProvider = GetComponent<IBallTouchInputProvider>();
        ballEffects = GetComponent<BallEffectsHandler>();
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
    }

    public void SetPipeIndex(int index, bool move)
    {
        _pipeIndex = index;
        if (move && _pipe)
        {
            _movementController.Move(_pipe.WaypointProvider.Waypoints[_pipeIndex]);
        }
    }

    public void DestroyBall()
    {
        OnBallDestroyed();
        Destroy(gameObject, ballDespawnTime);
    }

    //Override methods for child
    protected virtual void OnBallDestroyed()
    {
        eventHandler.BallScoreAdded(clearPoints);
        _movementController.FreeFall();
    }

    protected virtual void OnBallTouched()
    {
        eventHandler.BallTouched(this);
    }

    protected virtual void OnBallSelected(BallController ball) 
    { 
        if(_pipe is LoaderPipe) { return; }

        ballEffects.SetHighlight((ball != null && ball == this), (ball != null && ball.PipeIndex == _pipeIndex));
    }

    protected virtual void OnBallMovementFinished() { }
}
