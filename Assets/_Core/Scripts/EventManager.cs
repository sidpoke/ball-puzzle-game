using System;
using UnityEngine;

//EventBus Pattern
public class EventManager : MonoBehaviour
{
    /// <summary>
    /// This class serves for "globally called" events as a decoupling mechanism.
    /// Should any object want to know of what the events do, they can simply subscribe.
    /// 
    /// This is also known as the EventBus Pattern.
    /// And yes it's kind of inspired by how Godot handles things, i was curious if this works in Unity too.
    /// </summary>

    public event Action<BallController> BallTouched;
    //public event Action BallAdded;
    //public event Action<int> ScoreAdded;
    public event Action<int> ScoreUpdated;
    public event Action<PipeController> PipeChanged;

    public void EventScoreUpdate(int score)
    {
        ScoreUpdated?.Invoke(score);
    }

    public void EventBallTouched(BallController ball)
    {
        BallTouched?.Invoke(ball);
    }

    public void EventPipeChanged(PipeController pipe)
    {
        PipeChanged?.Invoke(pipe);
    }
}
