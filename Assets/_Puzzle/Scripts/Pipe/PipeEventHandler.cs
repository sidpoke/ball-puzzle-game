using UnityEngine;

public class PipeEventHandler : MonoBehaviour, IPipeEventHandler
{
    // This shoots out whenever something changes and does not send back any info

    public void PipeChanged(PipeController pipe)
    {
        GameService.Instance.eventManager.EventPipeChanged(pipe);
    }

    public void BallAdded(PipeController pipe, BallController ball)
    {
        //do it
    }

    public void BallMoved(PipeController pip, BallController ball)
    {
        //do it
    }

    public void BallRemoved(PipeController pipe, BallController ball)
    {
        //do it
    }
}