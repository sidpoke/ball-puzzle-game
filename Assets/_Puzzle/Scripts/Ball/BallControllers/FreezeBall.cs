using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Freeze Ball works like a regular AnyBall but triggers an event before destroying.
/// </summary>
public class FreezeBall : BallController 
{
    //Overrides the OnBallDestroyed method to trigger a special event when released.
    protected override void OnBallDestroyed()
    {
        base.OnBallDestroyed();
        TriggerSpecialEvent();
    }
}
