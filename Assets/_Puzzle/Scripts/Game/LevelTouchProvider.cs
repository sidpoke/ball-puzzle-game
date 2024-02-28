using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTouchProvider : MonoBehaviour
{
    public event Action<BallController, BallController> SwapBalls;

    [SerializeField]
    private BallController lastTouchedBall = null;

    void Start()
    {
        //subcribe to events
        GameService.Instance.eventManager.BallTouched += OnTouchResponseBallSwap;
    }

    /// <summary>
    /// On touch response -> Ball swap (also check for restrictions & setting highlights)
    /// </summary>
    /// <param name="ball"></param>
    private void OnTouchResponseBallSwap(BallController ball)
    {
        if (ball.Pipe is LoaderPipe || ball.movementController.IsMoving) //ignore loader pipe or moving ball
        {
            return;
        }


        if (lastTouchedBall && lastTouchedBall != ball && lastTouchedBall.Pipe != ball.Pipe && lastTouchedBall.PipeIndex == ball.PipeIndex) //second touch switch
        {
            SwapBalls?.Invoke(lastTouchedBall, ball);
            lastTouchedBall = null;
        }
        else if (lastTouchedBall && lastTouchedBall == ball) //deselect when ball is already selected
        {
            lastTouchedBall = null;
        }
        else //select the ball
        {
            lastTouchedBall = ball;
        }

        //Broadcast the ball that has been selected
        GameService.Instance.eventManager.Event_LevelBallSelected(lastTouchedBall);
    }
}
