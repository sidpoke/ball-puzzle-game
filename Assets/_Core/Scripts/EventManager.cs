using System;
using UnityEngine;

//EventBus Pattern
public class EventManager : MonoBehaviour
{
    /// <summary>
    /// This class serves for "globally called" events.
    /// Should any object want to know what other objects do, they can simply subscribe to the event.
    /// I want to separate actions and responses to make them dependent on its GameService logic.
    /// This only works because the actions within the game are known. The scripts themselves are mostly decoupled.
    /// </summary>

    //Level events
    public event Action<BallController> LevelBallSelected;

    //Pipe events
    public event Action<PipeController, BallController> PipeBallAdded;
    public event Action<PipeController, BallController> PipeBallMoved;
    public event Action<PipeController, BallController> PipeBallRemoved;

    //Ball events
    public event Action<BallController> BallTouched;
    public event Action<int> BallScoreAdded;

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
