using UnityEngine;

/// <summary>
/// This shoots out events whenever something changes within a pipe and it does not send back any info
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
