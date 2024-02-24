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
        PipeStorage.BallAdded += MoveBall;
    }

    public void MoveBall(BallController ball, int newIndex)
    {
        ball.transform.position = WaypointProvider.Waypoints[WaypointProvider.Waypoints.Length - 1 - ball.PipeIndex];
        Vector2[] newPositions = PipeControllerHelpers.WaypointsToBallMovement(WaypointProvider.Waypoints, ball.PipeIndex, newIndex);
        ball.SetPipeAndIndex(this, newIndex);
        ball.movementController.Move(newPositions);

    }
}