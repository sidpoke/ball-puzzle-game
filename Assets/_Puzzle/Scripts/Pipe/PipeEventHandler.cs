using UnityEngine;

public class PipeEventHandler : MonoBehaviour, IPipeEventHandler
{
    public void PipeChanged(PipeController pipe)
    {
        GameService.Instance.eventManager.EventPipeChanged(pipe);
    }
}