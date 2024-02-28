using System;
using UnityEngine;

public interface IBallMovementController
{
    public event Action FinishedMoving;
    public bool IsMoving { get; }

    public void Move(Vector2 position);
    public void Move(Vector2[] path);
    public void SpawnPosition(Vector2 position);
    public void Stop();
}
