using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSwitchBall : BallController 
{
    //NoSwitch does not send inputs, instead it should play an animation
    protected override void OnBallTouched()
    {
        animationController.PlayAnimation("BallWiggle");
        //play sound too!
    }
}
