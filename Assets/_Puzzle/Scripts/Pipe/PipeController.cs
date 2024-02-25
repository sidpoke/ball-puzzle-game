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

    public IPipeWaypointProvider WaypointProvider { get { return _waypointProvider; } }
    public IPipeStorageProvider PipeStorage { get { return _pipeStorage; } } 

    protected virtual void Awake()
    {
        _waypointProvider = GetComponent<IPipeWaypointProvider>();
        _pipeStorage = GetComponent<IPipeStorageProvider>();
        _pipeStorage.BallAdded += MoveNewBall;
        _pipeStorage.BallMoved += MoveDownBall;
        _pipeStorage.BallRemoved += RemoveBall;
    }

    public void MoveNewBall(BallController ball)
    {
        ball.transform.position = WaypointProvider.Waypoints[WaypointProvider.Waypoints.Length - 1];
        ball.movementController.Move(
            PipeControllerHelpers.WaypointsToBallMovement(WaypointProvider.Waypoints, ball.PipeIndex));
    }

    public void MoveDownBall(BallController ball)
    {
        ball.movementController.Move(WaypointProvider.Waypoints[ball.PipeIndex]);
    }

    public void RemoveBall(BallController ball)
    {
        //just to test
        Destroy(ball.gameObject);
    }
}