using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Slow Ball works like a regular Block-Ball but triggers an event when entered a pipe.
/// </summary>
public class BombBall : BallController 
{
    private ITimerProvider timer;
    private SpriteRenderer spriteRenderer;

    [Header("Bomb Setup")]
    [SerializeField] private float bombTriggerTime = 5.0f;
    [SerializeField] private float finalBombTickSpeed = 10.0f;

    [Header("Debug")]
    [SerializeField] private float bombTickPhase = 0f; //used for shader

    protected override void Awake()
    {
        base.Awake();
        timer = GetComponent<ITimerProvider>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.material.SetFloat("_BombTickSpeed", 0);
        timer.TimerReset();
        timer.SetTimerTime(bombTriggerTime);
    }

    public void Update()
    {
        if (timer.IsRunning)
        {
            bombTickPhase += Time.deltaTime;
            spriteRenderer.material.SetFloat("_BombTickPhase", bombTickPhase);
            spriteRenderer.material.SetFloat("_BombTickSpeed", timer.CurrentTime / bombTriggerTime * finalBombTickSpeed);
        }
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

    //BombBall detects a pipe change, as soon as drops inside a switcher pipe it will tick.
    protected override void OnBallMovementFinished()
    {
        base.OnBallMovementFinished();
        if(Pipe is SwitcherPipe)
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
