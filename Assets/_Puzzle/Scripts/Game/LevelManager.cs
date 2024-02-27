using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Level Manager is responsible for delivering actions to the GameManager that affect the level.
/// </summary>
public class LevelManager : MonoBehaviour
{
    private ITimerProvider _timerProvider;

    [SerializeField]
    private LoaderPipe loaderPipe;
    [SerializeField]
    private List<SwitcherPipe> pipes = new List<SwitcherPipe>();
    [SerializeField]
    private Transform ballPool;
    [SerializeField]
    private List<GameObject> ballPrefabs;

    private BallController lastTouchedBall;

    public List<SwitcherPipe> Pipes { get { return pipes; } }

    public void Awake()
    {
        _timerProvider = GetComponent<TimerProvider>();
    }

    public void Start()
    {
        GameService.Instance.eventManager.BallTouched += OnTouchResponseBallSwap;
        pipes.ForEach(pipe => { pipe.PipeStorage.BallRemoved += OnSwitcherPipeReleased; });
        loaderPipe.PipeStorage.BallRemoved += OnLoaderPipeReleased;
    }

    public void OnSwitcherPipeReleased(BallController ball)
    {
        //Ask LoaderPipe to release a ball when one was missing
        loaderPipe.OnSwitcherPipeReleased();
    }

    public void OnLoaderPipeReleased(BallController ball)
    {
        //put new ball into a free pipe
        foreach (SwitcherPipe pipe in pipes) 
        {
            if (!pipe.PipeStorage.IsFull)
            {
                ball.SetPipe(pipe);
                pipe.PipeStorage.Add(ball);
                return;
            }
        }
    }

    public void OnTouchResponseBallSwap(BallController ball)
    {
        if(ball.Pipe == loaderPipe || ball.movementController.IsMoving) //ignore loader pipe
        {
            return;
        }
        if(lastTouchedBall == null) //first touch select
        {
            lastTouchedBall = ball;
            return;
        }
        else if(lastTouchedBall != ball && lastTouchedBall.Pipe != ball.Pipe && lastTouchedBall.PipeIndex == ball.PipeIndex) //second touch switch
        {
            SwapBalls(lastTouchedBall, ball);
            lastTouchedBall = null;
        }
        else if (lastTouchedBall == ball) //second touch deselect
        {
            lastTouchedBall = null;
        }
    }

    public void FillAllPipes(/*float seed*/)
    {
        pipes.ForEach(pipe => FillPipe(pipe));
    }

    public void AddBallToLoader()
    {
        AddBallToPipe(loaderPipe);
    }

    public void ReleaseLoaderBall()
    {
        ReleaseBallFromPipe(loaderPipe);
    }

    public void KillRandomBall()
    {
        RemoveBallFromPipe(loaderPipe, Random.Range(0, loaderPipe.PipeStorage.Balls.Count));
    }

    /// <summary>
    /// Fills an entire pipe at the start with random balls. 
    /// 
    /// TODO: Scriptable objects overload for pretedermined level patterns
    /// </summary>
    /// <param name="pipe">The pipe to fill</param>
    public void FillPipe(PipeController pipe)
    {
        for (int i = 0; i < pipe.PipeStorage.MaxFillAmount; i++)
        {
            AddBallToPipe(pipe);
        }
    }

    /// <summary>
    /// Adds a single random ball to a Pipe
    /// </summary>
    /// <param name="pipe">The pipe to fill</param>
    public void AddBallToPipe(PipeController pipe)
    {
        if (!pipe.PipeStorage.IsFull)
        {
            int rand = Random.Range(0, ballPrefabs.Count);
            BallController ball = Instantiate(ballPrefabs[rand], transform.position, Quaternion.identity, ballPool).GetComponent<BallController>();
            ball.SetPipe(pipe);
            pipe.PipeStorage.Add(ball);
        }
        else
        {
            Debug.LogError($"Pipe \"{pipe.name}\" is already full!!");
        }
    }
    public void ReleaseBallFromPipe(PipeController pipe)
    {
        if (!pipe.PipeStorage.IsEmpty)
        {
            pipe.PipeStorage.Release();
        }
        else
        {
            Debug.LogError($"Pipe \"{pipe.name}\" is already Empty!!");
        }
    }

    public void RemoveBallFromPipe(PipeController pipe, int index)
    {
        if (!pipe.PipeStorage.IsEmpty)
        {
            pipe.PipeStorage.RemoveAt(index);
        }
        else
        {
            Debug.LogError($"Pipe \"{pipe.name}\" is already Empty!!");
        }
    }

    public void SwapBalls(BallController ballA, BallController ballB)
    {
        //switch elements between two pipes
        PipeController pipeA = ballA.Pipe;
        PipeController pipeB = ballB.Pipe;

        //store pipe index A
        int oldIndexA = ballA.PipeIndex;

        //Move Ball A to Pipe B with index B
        ballA.SetPipe(pipeB);
        ballA.SetPipeIndex(ballB.PipeIndex);
        pipeB.PipeStorage.Balls.RemoveAt(ballB.PipeIndex);
        pipeB.PipeStorage.Balls.Insert(ballB.PipeIndex, ballA);
        ballA.movementController.Move(pipeB.WaypointProvider.Waypoints[ballB.PipeIndex]);

        //Move Ball B to Pipe A with old index of A
        ballB.SetPipe(pipeA);
        ballB.SetPipeIndex(oldIndexA);
        pipeA.PipeStorage.Balls.RemoveAt(oldIndexA);
        pipeA.PipeStorage.Balls.Insert(oldIndexA, ballB);
        ballB.movementController.Move(pipeA.WaypointProvider.Waypoints[oldIndexA]);
    }
}
