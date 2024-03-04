using System;
using UnityEngine;

/// <summary>
/// LevelTouchProvider handles the Input within a level and and fires an event if balls are to be swapped 
/// </summary>
public class LevelTouchProvider : MonoBehaviour
{
    public event Action<BallController, BallController> SwapBalls;

    [Header("Debug")]
    [SerializeField] private BallController lastTouchedBall = null; //Saves last touched ball
    [SerializeField] private bool canTouch;

    private void OnEnable() //subcribe to events
    {     
        GameService.Instance.eventManager.BallTouched += OnTouchResponseBallSwap;
    }

    private void OnDisable() //unsubcribe events
    {
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
        //ignore loader pipe or moving ball
        if (!canTouch || ball.Pipe is LoaderPipe || ball.MovementController.IsMoving) 
        {
            return;
        }

        //second touch switch
        if (lastTouchedBall && lastTouchedBall != ball && lastTouchedBall.Pipe != ball.Pipe && lastTouchedBall.PipeIndex == ball.PipeIndex) 
        {
            SwapBalls?.Invoke(lastTouchedBall, ball); //two balls have been selected, 
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

        //Broadcast globally which ball has been selected
        //Could for example be subscribed by a highlighter-class on a ball
        GameService.Instance.eventManager.Event_LevelBallSelected(lastTouchedBall);
    }
}
