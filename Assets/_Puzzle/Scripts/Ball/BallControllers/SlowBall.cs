using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slow Ball works like a regular AnyBall but triggers an event before destroying.
/// </summary>
public class SlowBall : BallController 
{
    protected override void OnBallDestroyed() //Overrides the OnBallDestroyed method to trigger a special event when released.
    {
        base.OnBallDestroyed();
        TriggerSpecialEvent();
    }
}
