using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// The Level Manager is responsible for delivering actions to the GameManager that affect the level.
/// </summary>
public class LevelManager : MonoBehaviour
{
    public event Action LoaderPipeFull;

    private LevelTouchProvider levelTouchProvider;

    [SerializeField]
    private LoaderPipe loaderPipe;
    [SerializeField]
    private List<SwitcherPipe> pipes = new List<SwitcherPipe>();
    [SerializeField]
    private Transform ballPool;
    [SerializeField]
    private List<GameObject> ballPrefabs;

    //public getters
    public LoaderPipe LoaderPipe { get { return loaderPipe; } }
    public List<SwitcherPipe> Pipes { get { return pipes; } }
    public SwitcherPipe LeastFilledPipe { get { return pipes.OrderBy(pipe => pipe.PipeStorage.Balls.Count).First(); } }

    private void Awake()
    {
        levelTouchProvider = GetComponent<LevelTouchProvider>();
    }

    private void Start()
    {
        //subscribe to events
        GameService.Instance.eventManager.PipeBallRemoved += OnPipeReleased;
        levelTouchProvider.SwapBalls += OnBallSwap;
    }

    /// <summary>
    /// When LoaderPipe releases the ball, transferred to least filled SwitcherPipe
    /// When SwitcherPipe releases a ball, ask LoaderPipe to release a new ball
    /// </summary>
    /// <param name="pipe"></param>
    /// <param name="ball"></param>
    public void OnPipeReleased(PipeController originPipe, BallController ball)
    {
        if (originPipe is SwitcherPipe) //ignore if this pipe released the ball
        {
            loaderPipe.AddQueue();
        }

        if (originPipe is LoaderPipe)
        {
            SwitcherPipe leastFilledPipe = LeastFilledPipe;

            if (!leastFilledPipe.PipeStorage.IsFull)
            {
                leastFilledPipe.PipeStorage.Add(ball);
                ball.SetPipe(leastFilledPipe);
            }
        }
    }

    /// <summary>
    /// Switches a pair of balls between two pipes.
    /// </summary>
    /// <param name="ballA">First Ball</param>
    /// <param name="ballB">Second Ball</param>
    public void OnBallSwap(BallController ballA, BallController ballB)
    {
        //switch elements between two pipes
        PipeController pipeA = ballA.Pipe;
        PipeController pipeB = ballB.Pipe;

        //store pipe index A
        int oldIndexA = ballA.PipeIndex;

        //Move Ball A to Pipe B with index B
        ballA.SetPipe(pipeB);
        ballA.SetPipeIndex(ballB.PipeIndex, true);
        pipeB.PipeStorage.Balls.RemoveAt(ballB.PipeIndex);
        pipeB.PipeStorage.Balls.Insert(ballB.PipeIndex, ballA);
        ballA.MovementController.Move(pipeB.WaypointProvider.Waypoints[ballB.PipeIndex]);

        //Move Ball B to Pipe A with old index of A
        ballB.SetPipe(pipeA);
        ballB.SetPipeIndex(oldIndexA, true);
        pipeA.PipeStorage.Balls.RemoveAt(oldIndexA);
        pipeA.PipeStorage.Balls.Insert(oldIndexA, ballB);
        ballB.MovementController.Move(pipeA.WaypointProvider.Waypoints[oldIndexA]);
    }

    /// <summary>
    /// Fills all Switcher Pipes with random balls. 
    /// </summary>
    public void FillAllPipes()
    {
        pipes.ForEach(pipe => FillPipe(pipe));
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
            int rand = UnityEngine.Random.Range(0, ballPrefabs.Count);
            BallController ball = Instantiate(ballPrefabs[rand], transform.position, Quaternion.identity, ballPool).GetComponent<BallController>();
            ball.SetPipe(pipe);
            pipe.PipeStorage.Add(ball);
        }
        else if(pipe == loaderPipe) //loader pipe full? let game manager know
        { 
            {
                LoaderPipeFull?.Invoke();
            }
        }
    }
}
