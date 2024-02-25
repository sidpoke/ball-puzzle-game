using System.Linq;
using UnityEngine;

/// <summary>
/// Color Pipes only eject when color matches
/// </summary>

public enum PipeColor
{
    Red,
    Blue,
    Green,
    Yellow
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
    }
}
