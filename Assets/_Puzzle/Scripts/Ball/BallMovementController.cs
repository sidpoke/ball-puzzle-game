using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Movement controller moves the ball object to a specific position or a list of positions
/// Also can free fall when asked to.
/// </summary>
public class BallMovementController : MonoBehaviour, IBallMovementController
{
    public event Action FinishedMoving;

    [Header("Ball Movement Setup")]
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private bool canRotate;
    [SerializeField] private float rotateSpeed = 4.0f;
    [SerializeField] private float gravity = 9.81f;

    [Header("Debug")]
    [SerializeField] private List<Vector2> positions = new List<Vector2>();
    [SerializeField] private bool _moving = false;
    [SerializeField] private bool isSpawned = false;
    [SerializeField] private bool isFreeFalling = false;
    [SerializeField] private float freeFallTime = 0f;

    public bool IsMoving { get { return _moving || isFreeFalling; } }

    /// <summary>
    /// Adds a single position to the movement queue
    /// </summary>
    public void Move(Vector2 position)
    {
        positions.Add(position);
    }

    /// <summary>
    /// Adds a list of positions to the movement queue
    /// </summary>
    public void Move(Vector2[] path)
    {
        positions.AddRange(path);
    }

    /// <summary>
    /// Sets the ball to free fall mode, typically called when ball about to be destroyed
    /// </summary>
    public void FreeFall()
    {
        isFreeFalling = true;
    }

    /// <summary>
    /// Sets the spawn position of this ball (teleports once)
    /// </summary>
    public void SpawnPosition(Vector2 position)
    {
        if (!isSpawned) // teleport object to the spawn location
        {
            transform.position = position;
            isSpawned = true;
        }
        else
        {
            Move(position); // Add spawn point to the movement queue
        }
    }

    /// <summary>
    /// Stops movement queue
    /// </summary>
    public void Stop()
    {
        positions.Clear();
    }

    private void Update()
    {
        MovementQueue();
        RotationEffect();
        FreeFallEffect();
    }

    /// <summary>
    /// Adds downward acceleration
    /// </summary>
    public void FreeFallEffect()
    {
        if(isFreeFalling)
        {
            freeFallTime += Time.deltaTime;
            transform.Translate(0.5f * gravity * Vector2.down * Mathf.Pow(freeFallTime, 2), Space.World);
        }
    }

    /// <summary>
    /// Adds rotation based on X velocity if the object is moving
    /// </summary>
    public void RotationEffect()
    {
        if (canRotate && positions.Count > 0)
        {
            Vector2 moveDirX = positions[0] - (Vector2)transform.position;
            transform.eulerAngles += new Vector3(0, 0, -moveDirX.normalized.x * rotateSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Follows the first path in the queue then removes it when it's reached
    /// </summary>
    private void MovementQueue()
    {
        if (positions.Count > 0)
        {
            _moving = true;
            transform.position = Vector2.MoveTowards(transform.position, positions[0], moveSpeed * Time.deltaTime);
            if (transform.position == (Vector3)positions[0])
            {
                positions.RemoveAt(0);
            }
        }
        else if (_moving) // if done moving send out an event that others can subscribe to
        {
            FinishedMoving?.Invoke();
            _moving = false;
        }
    }
}
