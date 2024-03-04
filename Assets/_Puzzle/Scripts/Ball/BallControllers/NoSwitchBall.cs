using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NoSwitch Ball works like Any-Ball but does not send input events, instead it overrides the touch method and plays an animation
/// </summary>
public class NoSwitchBall : BallController 
{
    protected override void OnBallTouched()  //Override touch response
    {
        if(Pipe is not LoaderPipe && !MovementController.IsMoving) //not while moving or in loaderpipe
        {
            animationController.PlayAnimation("BallWiggle");
            audioController.PlayAudio("BallNoSwitch");
        }
    }
}
