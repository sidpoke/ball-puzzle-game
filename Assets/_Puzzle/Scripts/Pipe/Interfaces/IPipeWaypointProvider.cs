using UnityEngine;

public interface IPipeWaypointProvider
{
    public Vector2 SpawnPoint { get; }
    public Vector2 ScoreVFXSpawnPoint { get; }
    public Vector2[] Waypoints { get; }
    public void GenerateWaypoints(int amount);
}
