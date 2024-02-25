using System;
using System.Collections.Generic;
using System.Linq;
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

    public event Action<BallController> BallAdded;
    public event Action<BallController> BallMoved;
    public event Action<BallController> BallRemoved;

    public bool IsFull { get { return _balls.Count >= MaxFillAmount; } }
    public bool IsEmpty { get { return _balls.Count <= 0; } }

    public void Add(BallController ball)
    {
        if(_balls.Count >= MaxFillAmount)
        {
            Debug.LogError($"Pipe \"{name}\" is already full!!");
            return;
        }

        int ballIndex = _balls.Count;
        ball.SetPipeIndex(ballIndex == 0 ? 0 : ballIndex);

        _balls.Add(ball);
        BallAdded?.Invoke(ball);
    }

    public void InsertAt(int index, BallController ball)
    {
        if (_balls.Count >= MaxFillAmount)
        {
            Debug.LogError($"Pipe \"{name}\" is already full!!");
            return;
        }

        if (_balls.Count < index)
        {
            Debug.LogError($"Pipe \"{name}\" is not large enough for insert!!");
            return;
        }

        _balls.Insert(index, ball);
        //shift other balls index down by one
        _balls.ForEach(ball => {
            if (ball.PipeIndex > index)
            {
                ball.SetPipeIndex(ball.PipeIndex - 1);
                BallMoved?.Invoke(ball);
            }
        });
        BallAdded?.Invoke(ball);
    }

    public void Clear()
    {
        _balls.ForEach(ball =>
        {
            RemoveAt(0);
        });
    }

    public void Release()
    {
        RemoveAt(0);
    }

    public void RemoveAt(int index)
    {
        //Call event so this ball should be taken care of
        BallRemoved?.Invoke(_balls[index]);
        //shift other balls index down by one
        _balls.ForEach(ball => { 
            if (ball.PipeIndex > index) {
                ball.SetPipeIndex(ball.PipeIndex - 1);
                BallMoved?.Invoke(ball);
            }});
        //remove list entry
        _balls.RemoveAt(index);
    }

    public void RemoveRange(int index, int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            RemoveAt(index + i);
        }
    }
}
