using UnityEngine;

public interface IBallMovementController
{
    public void Move(Vector2 position);
    public void Move(Vector2[] path);
    public void Stop();
}