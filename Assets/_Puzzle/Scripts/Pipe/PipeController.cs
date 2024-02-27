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

        _pipeStorage.BallAdded += OnBallAdded;
        _pipeStorage.BallMoved += OnBallMoved;
        _pipeStorage.BallRemoved += OnBallRemoved;
    }

    protected virtual void OnBallAdded(BallController ball)
    {
        ball.movementController.SpawnPosition(WaypointProvider.SpawnPoint);
        ball.movementController.Move(
            PipeControllerHelpers.WaypointsToBallMovement(WaypointProvider.Waypoints, ball.PipeIndex));
        //eventHandler.OnBallAdded(this, ball);
    }

    protected virtual void OnBallMoved(BallController ball)
    {
        ball.movementController.Move(WaypointProvider.Waypoints[ball.PipeIndex]);
        //eventHandler.OnBallMoved(this, ball);
    }

    protected virtual void OnBallRemoved(BallController ball)
    {
        //eventHandler.OnBallRemoved(this, ball);
    }
}