using System.Linq;
using UnityEngine;

/// <summary>
/// Pipe Color to indicate which Ball Color can be released
/// </summary>
public enum PipeColor
{
    Red = 0,
    Green = 1,
    Blue = 2,
    Yellow = 3
}

/// <summary>
/// Switcher pipe contains balls that can be switched around.
/// Calls OnBallRemoved to destroy the ball when its released.
/// </summary>
public class SwitcherPipe : PipeController
{
    [Header("Color Pipe Setup")]
    [SerializeField]
    private PipeColor pipeColor = PipeColor.Red;

    public PipeColor PipeColor { get { return pipeColor; } }

    protected override void Awake()
    {
        base.Awake();
        WaypointProvider.GenerateWaypoints(PipeStorage.MaxFillAmount);
    }

    protected override void OnBallRemoved(BallController ball)
    {
        base.OnBallRemoved(ball);

        if(ball.Explode)
        {
            ball.ExplodeBall();  //kaboom!
        }
        else
        {
            ball.DestroyBall();  //This is your final call, mr. ball
        }
    }
}
