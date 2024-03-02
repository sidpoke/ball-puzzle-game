using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slow Ball works like a regular AnyBall but triggers an event before destroying.
/// </summary>
public class SlowBall : BallController 
{
    //NoSwitch does not send input events, instead it overrides the touch method and plays an animation
    protected override void OnBallDestroyed()
    {
        base.OnBallDestroyed();
        TriggerSpecialEvent();
    }
}
