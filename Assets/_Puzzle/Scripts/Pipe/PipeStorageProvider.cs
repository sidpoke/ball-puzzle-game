using System;
using System.Collections.Generic;
using UnityEngine;

public class PipeStorageProvider : MonoBehaviour, IPipeStorageProvider
{
    //private members hold storage
    [SerializeField]
    private int _maxFillAmount = 7;
    private List<BallController> _balls = new List<BallController>();

    //public getters
    public int MaxFillAmount { get { return _maxFillAmount; } }
    public List<BallController> Balls { get { return _balls; } }

    public event Action<BallController, int> BallAdded;
    public event Action<BallController, int> BallRemoved;

    public void Add(BallController ball)
    {
        if(_balls.Count >= MaxFillAmount)
        {
            Debug.LogError($"Pipe \"{name}\" is already full!!");
            return;
        }
        _balls.Add(ball);
        BallAdded?.Invoke(ball, _balls.Count);
    }

    public void Clear()
    {
        _balls.Clear();
    }

    public void Release()
    {
        _balls.RemoveAt(_balls.Count - 1);
    }

    public void RemoveAt(int index)
    {
        BallRemoved?.Invoke(Balls[index], index);
        _balls.RemoveAt(index);
    }

    public void RemoveRange(int index, int steps)
    {
        _balls.RemoveAt(index);
    }

    public void Switch(int index, IPipeStorageProvider otherPipe)
    {
        throw new System.NotImplementedException();
    }
}
