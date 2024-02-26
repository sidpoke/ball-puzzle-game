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
    protected IPipeWaypointProvider _waypointProvider;
    protected IPipeStorageProvider _pipeStorage;
    protected IPipeEventHandler _eventHandler;

    public IPipeWaypointProvider WaypointProvider { get { return _waypointProvider; } }
    public IPipeStorageProvider PipeStorage { get { return _pipeStorage; } }


    protected virtual void Awake()
    {
        _eventHandler = GetComponent<IPipeEventHandler>();
        _waypointProvider = GetComponent<IPipeWaypointProvider>();
        _pipeStorage = GetComponent<IPipeStorageProvider>();
        _pipeStorage.BallAdded += OnBallAdded;
        _pipeStorage.BallMoved += OnBallMoved;
    }

    public void OnBallAdded(BallController ball)
    {
        ball.movementController.SpawnPosition(WaypointProvider.Waypoints[WaypointProvider.Waypoints.Length - 1]);
        ball.movementController.Move(
            PipeControllerHelpers.WaypointsToBallMovement(WaypointProvider.Waypoints, ball.PipeIndex));
    }

    public void OnBallMoved(BallController ball)
    {
        ball.movementController.Move(WaypointProvider.Waypoints[ball.PipeIndex]);
    }
}
