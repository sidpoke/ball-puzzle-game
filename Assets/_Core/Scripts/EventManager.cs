using System;
using UnityEngine;

//EventBus Pattern
public class EventManager : MonoBehaviour
{
    /// <summary>
    /// Registers public event classes here that other objects can listen to when fired
    /// This class only serves for globally called events.
    /// Should any object want to know of the signals, they can simply subscribe.
    /// 
    /// This is also known as the EventBus Pattern.
    /// </summary>

    public event Action<BallController> BallTouched;
    //public event Action BallAdded;
    //public event Action BallRemoved;
    //public event Action<int> ScoreUpdated;
    public event Action<PipeController> PipeChanged;

    public void EventBallTouched(BallController ball)
    {
        BallTouched?.Invoke(ball);
    }

    public void EventPipeChanged(PipeController pipe)
    {
        PipeChanged?.Invoke(pipe);
    }
}
