using UnityEngine;

/// <summary>
/// A PipeController is an component that combines pipe-related subcomponents within a controller class.
/// Contains override methods for child classes.
/// </summary>
public class PipeController : MonoBehaviour
{
    protected IPipeEventHandler eventHandler; //sends out events globally
    protected IPipeWaypointProvider _waypointProvider; //generates waypoints for balls to move along to
    protected IPipeStorageProvider _pipeStorage; //provides a ball storage method for pipes

    public IPipeWaypointProvider WaypointProvider { get { return _waypointProvider; } }
    public IPipeStorageProvider PipeStorage { get { return _pipeStorage; } }

    protected virtual void Awake()
    {
        eventHandler = GetComponent<IPipeEventHandler>();
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
        eventHandler.PipeBallAdded(this, ball);
    }

    protected virtual void OnBallRemoved(BallController ball)
    {
        eventHandler.PipeBallRemoved(this, ball);
    }
}
