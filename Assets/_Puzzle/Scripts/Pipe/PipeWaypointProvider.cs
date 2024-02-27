using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PipeWaypointProvider : MonoBehaviour, IPipeWaypointProvider
{
    [SerializeField]
    private List<Transform> predeterminedWaypoints; //does the job
    [SerializeField]
    private Transform spawnPoint;

    // generated waypoints
    private Vector2[] _waypoints;

    // public getters
    public Vector2[] Waypoints { get { return _waypoints; } }
    public Vector2 SpawnPoint { get { return (Vector2)spawnPoint.position; } }

    public void GenerateWaypoints(int amount)
    {
        _waypoints = predeterminedWaypoints.Select(point => (Vector2)point.position).ToArray();
    }
}
