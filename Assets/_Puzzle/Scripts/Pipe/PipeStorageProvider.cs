using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class PipeStorageProvider : MonoBehaviour, IPipeStorageProvider
{
    //Events to broadcast upwards to the PipeController.
    //Could technically be solved with Controller injection too, or GetComponent<>.
    public event Action<BallController> BallAdded;
    public event Action<BallController> BallRemoved;

    //private members
    [SerializeField]
    private int _maxFillAmount = 7;
    [SerializeField, ReadOnly]
    private List<BallController> _balls = new List<BallController>();

    //public getters
    public int MaxFillAmount { get { return _maxFillAmount; } }
    public List<BallController> Balls { get { return _balls; } }
    public bool IsFull { get { return _balls.Count >= MaxFillAmount; } }
    public bool IsEmpty { get { return _balls.Count <= 0; } }

    public void Add(BallController ball)
    {
        if(_balls.Count >= MaxFillAmount)
        {
            Debug.LogError($"Pipe \"{name}\" is already full!!");
            return;
        }

        _balls.Add(ball);
        ball.SetPipeIndex(_balls.Count - 1, false);
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

        _balls.ForEach(ball => {
            if (ball.PipeIndex > index)
            {
                ball.SetPipeIndex(ball.PipeIndex - 1, true);
            }
        }); 
        
        _balls.Insert(index, ball);
        //shift other balls index down by one

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
        //shift other balls index down by one
        _balls.ForEach(ball => { 
            if (ball.PipeIndex > index) {
                ball.SetPipeIndex(ball.PipeIndex - 1, true);
            }});        
        
        //Call event so this ball should be taken care of
        BallRemoved?.Invoke(_balls[index]);
        
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
