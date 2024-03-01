using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// A pipe is an object that contains a list of balls
/// It also contains a list of waypoints to align balls with
/// 
/// IPipeStorageProvider takes care of all the storage functions
/// IPipeWaypointProvider gives the pipe waypoints
/// </summary>

public class PipeController : MonoBehaviour
{
    protected IPipeEventHandler _eventHandler;
    protected IPipeWaypointProvider _waypointProvider;
    protected IPipeStorageProvider _pipeStorage;

    public IPipeWaypointProvider WaypointProvider { get { return _waypointProvider; } }
    public IPipeStorageProvider PipeStorage { get { return _pipeStorage; } }


    protected virtual void Awake()
    {
        _eventHandler = GetComponent<IPipeEventHandler>();
        _waypointProvider = GetComponent<IPipeWaypointProvider>();
        _pipeStorage = GetComponent<IPipeStorageProvider>();

        WaypointProvider.GenerateWaypoints(PipeStorage.MaxFillAmount);
    }

    protected virtual void OnEnable()
    {
        //Subscribe to events
        _pipeStorage.BallAdded += OnBallAdded;
        _pipeStorage.BallRemoved += OnBallRemoved;
    }

    protected virtual void OnDisable()
    {
        //unsubscribe events
        _pipeStorage.BallAdded -= OnBallAdded;
        _pipeStorage.BallRemoved -= OnBallRemoved;
    }

    protected virtual void OnBallAdded(BallController ball)
    {
        ball.MovementController.SpawnPosition(WaypointProvider.SpawnPoint);
        ball.MovementController.Move(
            PipeControllerHelpers.WaypointsToBallMovement(WaypointProvider.Waypoints, ball.PipeIndex));
        _eventHandler.PipeBallAdded(this, ball);
    }

    protected virtual void OnBallRemoved(BallController ball)
    {
        _eventHandler.PipeBallRemoved(this, ball);
    }
}
