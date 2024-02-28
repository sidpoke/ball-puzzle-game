using System;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class BallMovementController : MonoBehaviour, IBallMovementController
{
    public event Action FinishedMoving;

    [SerializeField]
    private List<Vector2> positions = new List<Vector2>();
    [SerializeField]
    private float moveSpeed = 2.0f;
    [SerializeField]
    private float rotateSpeed = 4.0f;

    private bool _moving = false;
    public bool IsMoving { get { return _moving; } }

    private bool isSpawned = false;

    public void Move(Vector2 position) //Adds a single position to the movement queue
    {
        positions.Add(position);
    }

    public void Move(Vector2[] path) //Overload: Adds an array of positions to the movement queue 
    {
        positions.AddRange(path);
    }

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

    public void Stop()
    {
        positions.Clear();
    }

    private void Update()
    {
        MovementQueue();
        RotationEffect();
    }

    /// <summary>
    /// Adds rotation based on X velocity if the object is moving
    /// </summary>
    public void RotationEffect()
    {
        if (positions.Count > 0)
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
        else if (_moving)
        {
            FinishedMoving?.Invoke();
            _moving = false;
        }
    }
}
