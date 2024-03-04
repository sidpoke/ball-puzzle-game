using UnityEngine;

/// <summary>
/// Events that a PipeController can trigger
/// </summary>
public interface IPipeEventHandler
{
    public void PipeBallAdded(PipeController pipe, BallController ball);
    public void PipeBallRemoved(PipeController pipe, BallController ball);
}