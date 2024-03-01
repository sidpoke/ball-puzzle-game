using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTouchProvider : MonoBehaviour
{
    public event Action<BallController, BallController> SwapBalls;

    [SerializeField] private BallController lastTouchedBall = null;
    [SerializeField] private bool canTouch;


    private void OnEnable()
    {
        //subcribe to events
        GameService.Instance.eventManager.BallTouched += OnTouchResponseBallSwap;
    }

    private void OnDisable()
    {
        //unsubcribe events
        GameService.Instance.eventManager.BallTouched -= OnTouchResponseBallSwap;
    }


    public void SetCanTouch(bool active)
    {
        canTouch = active;
    }

    /// <summary>
    /// On touch response -> Ball swap (also check for restrictions & setting highlights)
    /// </summary>
    /// <param name="ball"></param>
    private void OnTouchResponseBallSwap(BallController ball)
    {
        if (!canTouch || ball.Pipe is LoaderPipe || ball.MovementController.IsMoving) //ignore loader pipe or moving ball
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
