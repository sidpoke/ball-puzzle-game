using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PipeWaypoint_Dumb : MonoBehaviour, IPipeWaypointProvider
{
    // dumb waypoint non-generator (i want to automate this)
    private Vector2[] _waypoints;
    public Vector2[] Waypoints { get { return _waypoints; } }

    [SerializeField]
    private List<Transform> predeterminedWaypoints; //does the job but looks like doo doo feces

    public void GenerateWaypoints(int amount)
    {
        _waypoints = predeterminedWaypoints.Select(point => (Vector2)point.position).ToArray();
    }
}
