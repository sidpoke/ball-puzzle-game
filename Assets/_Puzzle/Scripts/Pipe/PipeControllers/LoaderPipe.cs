using System;
using System.Linq;
using UnityEngine;

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
