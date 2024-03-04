using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Implementation of a waypoint provider. Generates waypoints out of a list that a PipeController can access.
/// </summary>
public class PipeWaypointProvider : MonoBehaviour, IPipeWaypointProvider
{
    [SerializeField] private List<Transform> predeterminedWaypoints;

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _scoreVFXSpawnPoint;

    // generated waypoints
    private Vector2[] _waypoints;

    // public getters
    public Vector2[] Waypoints { get { return _waypoints; } }
    public Vector2 SpawnPoint { get { return (Vector2)_spawnPoint.position; } }
    public Vector2 ScoreVFXSpawnPoint { get { return (Vector2)_scoreVFXSpawnPoint.position; } }

    public void GenerateWaypoints(int amount)
    {
        _waypoints = predeterminedWaypoints.Select(point => (Vector2)point.position).ToArray();
    }
}
