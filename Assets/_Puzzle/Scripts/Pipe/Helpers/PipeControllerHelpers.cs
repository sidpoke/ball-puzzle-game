using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class PipeControllerHelpers
{
    /// <summary>
    /// This method returns an array of positions that a ball will follow to position itself within a pipe.
    /// 
    /// </summary>
    /// <param name="fromIndex">Where the Ball will originate from within the pipe.</param>
    /// <param name="toIndex">Wherre the Ball will land within the pipe</param>
    /// <returns></returns>
    public static Vector2[] WaypointsToBallMovement(Vector2[] waypoints, int toIndex)
    {
        return waypoints.Reverse().Take(waypoints.Length - toIndex).ToArray();
    }
}