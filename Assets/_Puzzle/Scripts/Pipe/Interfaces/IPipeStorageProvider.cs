using System;
using System.Collections.Generic;

public interface IPipeStorageProvider
{
    /// <summary>
    /// BallAdded: BallController Instance, int InsertedAtIndex
    /// BallRemoved: BallController Instance, int RemovedAtIndex
    /// </summary>
    public event Action<BallController> BallAdded;
    public event Action<BallController> BallMoved;
    public event Action<BallController> BallRemoved;

    public List<BallController> Balls { get; }
    public int MaxFillAmount { get; }

    public bool IsFull { get; }
    public bool IsEmpty { get; }

    public void Add(BallController ball); //Add from the top
    public void Release(); //Release the lowest balls
    public void Clear(); //Release all balls
    public void RemoveAt(int index); //Release ball at index
    public void RemoveRange(int index, int steps); //Remove range at index
    public void Switch(int index, IPipeStorageProvider otherPipe); //Switch with a pipe
}