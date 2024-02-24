using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoaderPipe : PipeController
{
    protected override void Awake()
    {
        base.Awake();
        WaypointProvider.GenerateWaypoints(PipeStorage.MaxFillAmount);
    }
}
