using System;
using System.Linq;
using UnityEngine;

public class LoaderPipe : PipeController
{
    [SerializeField]
    private int queue;

    protected override void Awake()
    {
        base.Awake();
        WaypointProvider.GenerateWaypoints(PipeStorage.MaxFillAmount);
    }

    public void Update()
    {
        if (queue > 0 && !PipeStorage.IsEmpty)
        {
            PipeStorage.Release();
            queue--;
        }
    }

    public void OnSwitcherPipeReleased()
    {
        queue++;
    }
}
