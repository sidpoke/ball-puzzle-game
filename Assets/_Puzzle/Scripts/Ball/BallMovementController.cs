using System.Collections.Generic;
using UnityEngine;

public class BallMovementController : MonoBehaviour, IBallMovementController
{
    [SerializeField]
    private List<Vector2> positions = new List<Vector2>();
    [SerializeField]
    private float moveSpeed = 2.0f;

    public void Move(Vector2 position)
    {
        positions.Add(position);
        //Debug.Log($"Added Positions {position.ToString()} to object {name}");
    }

    public void Move(Vector2[] path)
    {
        positions.AddRange(path);
        //Debug.Log($"Added Positions {string.Join(", ", path)} to object {name}");
    }

    public void Stop()
    {
        positions.Clear();
    }

    void Update()
    {
        if(positions.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, positions[0], moveSpeed * Time.deltaTime);
            if (transform.position == (Vector3)positions[0])
            {
                positions.RemoveAt(0);
            }
        }
    }
}
