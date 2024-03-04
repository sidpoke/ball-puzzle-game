using UnityEngine;

/// <summary>
/// Laser ball has a timer and once it enters a SwitcherPipe it will tick.
/// If the timer runs out it this class will call the LevelManager to explode using its Pipe & PipeIndex.
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
        timer.TimerReset();
        timer.SetTimerTime(laserTriggerTime);
    }

    private void Start()
    {
        effectsController.SetBombTick(0, 0); //reset shader
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
        if (timer.IsRunning) //setting material properties for the bomb-tick-shader
        {
            laserTickPhase += Time.deltaTime;
            effectsController.SetBombTick(laserTickPhase, timer.CurrentTime / laserTriggerTime * finalLaserTickSpeed);
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

    public void OnTimerTimeout() //zap
    {
        TriggerSpecialEvent();
        audioController.PlayAudio("BallExplode");
    }
}
