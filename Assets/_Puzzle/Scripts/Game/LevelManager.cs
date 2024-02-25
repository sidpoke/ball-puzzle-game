using System.Linq;
using System.Collections.Generic;
using UnityEngine;

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

    public void Awake()
    {
        _timerProvider = GetComponent<TimerProvider>();
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

    public void AddBallToPipe(PipeController pipe)
    {
        if (!pipe.PipeStorage.IsFull)
        {
            int rand = Random.Range(0, ballPrefabs.Count);
            BallController ball = Instantiate(ballPrefabs[rand], transform.position, Quaternion.identity, ballPool).GetComponent<BallController>();
            int ballIndex = pipe.PipeStorage.Balls.Count;
            ball.SetPipeIndex(ballIndex == 0 ? 0 : ballIndex);
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
}
