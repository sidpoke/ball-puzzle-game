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
        if (pipe.PipeStorage.Balls.Count >= pipe.PipeStorage.MaxFillAmount)
        {
            Debug.LogError($"Pipe \"{name}\" is already full!!");
            return;
        }

        int rand = Random.Range(0, ballPrefabs.Count);
        GameObject go = Instantiate(ballPrefabs[rand], transform.position, Quaternion.identity, ballPool);
        pipe.PipeStorage.Add(go.GetComponent<BallController>());
    }
}
