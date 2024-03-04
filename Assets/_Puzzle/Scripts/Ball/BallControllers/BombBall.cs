using UnityEngine;

/// <summary>
/// Bomb ball has a timer and once it enters a SwitcherPipe it will tick.
/// If the timer runs out it this class will call the LevelManager to explode using its Pipe & PipeIndex.
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
        timer.TimerReset();
        timer.SetTimerTime(bombTriggerTime);
    }

    private void Start()
    {
        effectsController.SetBombTick(0, 0); //reset shader
    }

    protected override void OnEnable() //subscribe to events
    {
        base.OnEnable();
        timer.Timeout += OnTimerTimeout;
    }

    protected override void OnDisable() //unsubscribe events
    {
        base.OnDisable();
        timer.Timeout -= OnTimerTimeout;
    }

    public void Update()
    {
        if (timer.IsRunning) //setting material properties for the bomb-tick-shader
        {
            bombTickPhase += Time.deltaTime; //phase to make the bomb tick effect work
            effectsController.SetBombTick(bombTickPhase, timer.CurrentTime / bombTriggerTime * finalBombTickSpeed);
        }
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

    public void OnTimerTimeout() //boom
    {
        TriggerSpecialEvent();
        audioController.PlayAudio("BallExplode");
    }
}
