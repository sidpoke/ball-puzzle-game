using UnityEngine;

/// <summary>
/// Called by PipeController to send events globally
/// </summary>
public class PipeEventHandler : MonoBehaviour, IPipeEventHandler
{
    public void PipeBallAdded(PipeController pipe, BallController ball)
    {
        GameService.Instance.eventManager.Event_PipeBallAdded(pipe, ball);
    }

    //Careful, ball might already be destroyed.
    public void PipeBallRemoved(PipeController pipe, BallController ball)
    {
        GameService.Instance.eventManager.Event_PipeBallRemoved(pipe, ball);
    }
}
