using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using Unity.Collections;

/// <summary>
/// A Ball Prefab combined with a weight
/// </summary>
[System.Serializable]
public struct BallObject
{
    public GameObject Prefab;
    [Range(0, 100)] public int Weight;
}

/// <summary>
/// The LevelManager is responsible for handling changes within the game scene (Pipes and Balls).
/// It spawns Balls, fills pipes and swaps balls on call.
/// Purpose: The GameManager can call the levelmanager to change the game state.
/// </summary>
public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// Event is fired in case the loader pipe is full. This could be a game over call for example.
    /// </summary>
    public event Action LoaderPipeFull;

    private LevelTouchProvider levelTouchProvider;

    [Header("Ball Setup")]
    [SerializeField] private List<BallObject> ballObjects;

    [Header("Level References")]
    [SerializeField] private LoaderPipe loaderPipe;
    [SerializeField] private List<SwitcherPipe> pipes = new List<SwitcherPipe>();
    [SerializeField] private Transform _ballPool;

    [Header("Debug")]
    [SerializeField] private int[] ballWeights;

    //public getters
    public BallController[] Balls { get { return _ballPool.GetComponentsInChildren<BallController>(); } } //Don't call this too much
    public LoaderPipe LoaderPipe { get { return loaderPipe; } }
    public List<SwitcherPipe> Pipes { get { return pipes; } }
    public SwitcherPipe LeastFilledPipe { get { return pipes.OrderBy(pipe => pipe.PipeStorage.Balls.Count).First(); } }

    private void Awake()
    {
        levelTouchProvider = GetComponent<LevelTouchProvider>();
        ballWeights = ballObjects.Select(ball => ball.Weight).ToArray(); // extract ball weights, needed for random calculation
    }

    private void OnEnable()
    {
        //subscribe to events
        GameService.Instance.eventManager.PipeBallRemoved += OnPipeReleased;
        levelTouchProvider.SwapBalls += OnBallSwap;
    }

    private void OnDisable()
    {
        //unsubscribe events
        GameService.Instance.eventManager.PipeBallRemoved -= OnPipeReleased;
        levelTouchProvider.SwapBalls -= OnBallSwap;
    }

    public void SetCanTouch(bool active)
    {
        levelTouchProvider.SetCanTouch(active);
    }

    /// <summary>
    /// Destroys balls in a 9-block, used for bombs.
    /// </summary>
    public void DestroyBallsNineBlock(BallController triggerBall)
    {
        if(triggerBall.Pipe is SwitcherPipe switcherPipe)
        {
            int pipeX = pipes.IndexOf(switcherPipe); //get x position of ball
            int pipeY = triggerBall.PipeIndex; //get y position of ball

            for (int x = pipeX - 1;x <= pipeX + 1; x++) //start one index below, stop one index after
            {
                if(x >= 0 && x < pipes.Count) //ignore indices that don't exist, e.g. -1 and greater than pipes.Count
                {
                    for (int y = pipeY + 1; y >= pipeY - 1; y--) //same thing for y but top to bottom (avoid index shifting issues)
                    {
                        if(y >= 0 && y < pipes[x].PipeStorage.Balls.Count)
                        {
                            pipes[x].PipeStorage.Balls[y].SetExplode(true);
                            pipes[x].PipeStorage.RemoveAt(y);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Destroys balls in a line, used for lasers.
    /// </summary>
    public void DestroyBallsLine(BallController triggerBall)
    {
        if (triggerBall.Pipe is SwitcherPipe switcherPipe)
        {
            pipes.ForEach(pipe =>
            {
                if(pipe.PipeStorage.Balls.Count > triggerBall.PipeIndex) //if not out of range
                {
                    pipe.PipeStorage.Balls[triggerBall.PipeIndex].SetExplode(true);
                    pipe.PipeStorage.RemoveAt(triggerBall.PipeIndex);
                }
            });
        }
    }


    /// <summary>
    /// When LoaderPipe releases the ball, transfer to least filled SwitcherPipe
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
    /// Clears all pipes and destroys all balls in the scene.
    /// </summary>
    public void ClearAllPipes()
    {
        pipes.ForEach(pipe => pipe.PipeStorage.Clear());
        loaderPipe.PipeStorage.Clear();
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
            BallController ball = Instantiate(CalculateRandomWeightedBall(ballWeights), transform.position, Quaternion.identity, _ballPool.transform).GetComponent<BallController>();
            ball.SetPipe(pipe);
            pipe.PipeStorage.Add(ball);
        }
        else if(pipe == loaderPipe)
        { 
            LoaderPipeFull?.Invoke(); //loader pipe full? let game manager know
        }
    }

    /// <summary>
    /// Calculates a random weighted ball from the list.
    /// </summary>
    /// <returns>The random prefab to instantiate.</returns>
    //Reference: https://limboh27.medium.com/implementing-weighted-rng-in-unity-ed7186e3ff3b
    private GameObject CalculateRandomWeightedBall(int[] weights)
    {
        int randomWeight = UnityEngine.Random.Range(0, ballWeights.Sum());

        for (int i = 0; i < weights.Length; i++)
        {
            randomWeight -= weights[i];
            if(randomWeight < 0)
            {
                return ballObjects[i].Prefab;
            }
        }

        return ballObjects[0].Prefab;
    }
}
