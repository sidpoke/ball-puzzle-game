using System;
using UnityEngine;

/// <summary>
/// EventBus / Global event manager class that has events that other scripts can subscribe to from anywhere
/// </summary>
public class EventManager : MonoBehaviour
{
    // This class serves for "globally called" events.
    // Should any object want to know what other objects do, they can simply subscribe to and fire events here.
    // This only works because the actions within the game are known. The components themselves are supposed to be decoupled.
    // Could also have been solved using Scriptable Objects instead of Actions, but C# actions seemed more convenient.
    // It's important that these actions need to be unsubscribed because Unity doesn't automagically disconnect them.
    // By unsubscribing we ensure that the game stays bug-free and don't cause a memory leak.

    //Level events
    public event Action<BallController> LevelBallSelected;

    //Pipe events
    public event Action<PipeController, BallController> PipeBallAdded;
    public event Action<PipeController, BallController> PipeBallMoved;
    public event Action<PipeController, BallController> PipeBallRemoved;

    //Ball events
    public event Action<BallController> BallTouched;
    public event Action<int> BallScoreAdded;
    public event Action<BallController, BallSpecialEvent> BallSpecialTriggered;

    public void Event_LevelBallSelected(BallController ball)
    {
        LevelBallSelected?.Invoke(ball);
    }

    public void Event_BallTouched(BallController ball)
    {
        BallTouched?.Invoke(ball);
    }

    public void Event_BallScoreAdded(int score)
    {
        BallScoreAdded?.Invoke(score);
    }

    public void Event_BallSpecialEvent(BallController ball, BallSpecialEvent specialEvent)
    {
        BallSpecialTriggered?.Invoke(ball, specialEvent);
    }

    public void Event_PipeBallAdded(PipeController pipe, BallController ball)
    {
        PipeBallAdded?.Invoke(pipe, ball);
    }

    public void Event_PipeBallMoved(PipeController pipe, BallController ball)
    {
        PipeBallMoved?.Invoke(pipe, ball);
    }

    public void Event_PipeBallRemoved(PipeController pipe , BallController ball)
    {
        PipeBallRemoved?.Invoke(pipe, ball);
    }
}
