using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slow Ball works like a regular AnyBall but triggers an event before destroying.
/// </summary>
public class LaserBall : BallController 
{
    private ITimerProvider timer;

    [Header("Laser Setup")]
    [SerializeField] private float laserTriggerTime = 5.0f;

    protected override void Awake()
    {
        base.Awake();
        timer = GetComponent<ITimerProvider>();
        timer.SetTimerTime(laserTriggerTime);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        timer.Timeout += OnTimerTimeout;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        timer.Timeout -= OnTimerTimeout;
    }

    //LaserBall detects a pipe change, as soon as it enters a switcher pipe it will tick.
    protected override void OnBallPipeChanged()
    {
        base.OnBallPipeChanged();
        if (Pipe is SwitcherPipe)
        {
            timer.TimerStart(); //wait for a timer to run out before trigger
        }
    }
    public void OnTimerTimeout()
    {
        TriggerSpecialEvent();
        audioController.PlayAudio("BallExplode");
    }
}
