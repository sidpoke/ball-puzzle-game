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
    public static Vector2[] WaypointsToBallMovement(Vector2[] waypoints, int fromIndex, int toIndex)
    {
        List<Vector2> result = new List<Vector2>();

        int steps = toIndex - fromIndex;
        int length = waypoints.Length + 1;

        if (steps > 0)
        {
            return waypoints.Reverse().Skip(fromIndex).Take(length - steps).ToArray();
        }
        else if (steps < 0)
        {
            return waypoints.Take(length - Mathf.Abs(steps)).ToArray();
        }
        else
        {
            return null;
        }
    }
}