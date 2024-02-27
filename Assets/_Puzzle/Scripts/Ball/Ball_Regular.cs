using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallColor
{
    Red = 0,
    Green = 1, 
    Blue = 2,
    Yellow = 3,
    Any = 4
}

public class Ball_Regular : BallController
{
    [SerializeField]
    private BallColor _color;

    public BallColor BallColor { get { return _color; } }   

    protected override void Awake()
    {
        base.Awake();
        touchInputProvider.BallTouched += OnBallTouched;
        movementController.FinishedMoving += OnBallMovementFinished;
    }

    /// <summary>
    /// Called from LevelManager when selected
    /// </summary>
    public void OnBallSelected()
    {

    }

    private void OnBallTouched()
    {
        eventHandler.EventBallTouched(this);

        GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color == Color.gray ? Color.white : Color.gray;
    }

    private void OnBallMovementFinished()
    {
        CheckForColorMatch();
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void CheckForColorMatch()
    {
        if (Pipe is SwitcherPipe pipe && PipeIndex == 0)
        {
            if((int)BallColor == (int)pipe.PipeColor || BallColor == BallColor.Any)
            {
                pipe.PipeStorage.Release();
            }
        }
    }
}
