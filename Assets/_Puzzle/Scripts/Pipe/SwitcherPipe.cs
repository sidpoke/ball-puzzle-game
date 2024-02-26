using System.Linq;
using UnityEngine;

/// <summary>
/// Color Pipes only eject when color matches
/// </summary>

public enum PipeColor
{
    Red = 0,
    Green = 1,
    Blue = 2,
    Yellow = 3
}

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
        _pipeStorage.BallRemoved += OnBallRemoved;
    }

    public void OnBallRemoved(BallController ball)
    {
        //This is your final call, mr. ball
        ball.DestroyBall();
    }
}
