using UnityEngine;

public interface IPipeEventHandler
{
    //Events that a pipe triggers
    public void PipeBallAdded(PipeController pipe, BallController ball);
    public void PipeBallRemoved(PipeController pipe, BallController ball);
}