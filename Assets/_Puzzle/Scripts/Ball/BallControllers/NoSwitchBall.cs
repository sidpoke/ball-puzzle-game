using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSwitchBall : BallController 
{
    //NoSwitch does not send input events, instead it overrides the touch method and plays an animation
    protected override void OnBallTouched()
    {
        if(Pipe is not LoaderPipe && !MovementController.IsMoving)
        {
            animationController.PlayAnimation("BallWiggle");
            audioController.PlayAudio("BallNoSwitch");
        }
    }
}
