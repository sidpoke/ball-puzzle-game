using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slow Ball works like a regular AnyBall but triggers an event before destroying.
/// </summary>
public class LaserBall : BallController 
{
    private ITimerProvider timer;
    private SpriteRenderer spriteRenderer;

    [Header("Laser Setup")]
    [SerializeField] private float laserTriggerTime = 5.0f;
    [SerializeField] private float finalLaserTickSpeed = 10.0f;

    [Header("Debug")]
    [SerializeField] private float laserTickPhase = 0f; //used for shader

    protected override void Awake()
    {
        base.Awake();
        timer = GetComponent<ITimerProvider>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.material.SetFloat("_BombTickSpeed", 0);
        timer.TimerReset();
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

    public void Update()
    {
        if (timer.IsRunning)
        {
            laserTickPhase += Time.deltaTime;
            spriteRenderer.material.SetFloat("_BombTickPhase", laserTickPhase);
            spriteRenderer.material.SetFloat("_BombTickSpeed", (timer.CurrentTime / laserTriggerTime) * finalLaserTickSpeed);
        }
    }

    //LaserBall detects a pipe change, as soon as drops inside a switcher pipe it will tick.
    protected override void OnBallMovementFinished()
    {
        base.OnBallMovementFinished();
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
