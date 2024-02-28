using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public IBallEventHandler eventHandler;
    public IBallMovementController movementController;
    public IBallTouchInputProvider touchInputProvider;

    [Header("Ball Setup")]
    [SerializeField]
    private int clearPoints = 100;

    [Header("Debug"),SerializeField]
    private PipeController _pipe;
    [SerializeField]
    private int _pipeIndex = 0;

    public PipeController Pipe { get { return _pipe; } }
    public int PipeIndex { get { return _pipeIndex;} }

    protected virtual void Awake()
    {
        eventHandler = GetComponent<IBallEventHandler>();
        movementController = GetComponent<IBallMovementController>();
        touchInputProvider = GetComponent<IBallTouchInputProvider>();

        touchInputProvider.BallTouched += OnBallTouched;
        movementController.FinishedMoving += OnBallMovementFinished;
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
            movementController.Move(_pipe.WaypointProvider.Waypoints[_pipeIndex]);
        }
    }

    public void DestroyBall()
    {
        OnBallDestroyed();
        Destroy(gameObject);
    }

    //Override methods for child
    protected virtual void OnBallDestroyed()
    {
        eventHandler.BallScoreAdded(clearPoints);
    }

    protected virtual void OnBallTouched()
    {
        eventHandler.BallTouched(this);
    }

    protected virtual void OnBallSelected() { } //Called from LevelManager

    protected virtual void OnBallMovementFinished() { }
}
