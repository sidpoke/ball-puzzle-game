using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// LoaderPipe is the Pipe that Balls initially land in before being inserted into SwitcherPipes 
/// Contains a queue that releases balls when loaded
/// </summary>
public class LoaderPipe : PipeController
{
    private int queue;

    private void Update()
    {
        QueueHandler();
    }

    private void QueueHandler()
    {
        if (queue > 0 && !PipeStorage.IsEmpty)
        {
            PipeStorage.Release();
            queue--;
        }
    }

    public void AddQueue()
    {
        queue++;
    }
}
