using UnityEngine;

public interface IPipeEventHandler
{
    //Events that a pipe triggers
    public void PipeChanged(PipeController pipe);
}