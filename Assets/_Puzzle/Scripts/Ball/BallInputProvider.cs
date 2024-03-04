using System;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Listens to the global event for touch responses and triggers when the ball is inside of the radius provided
/// </summary>
public class BallInputProvider : MonoBehaviour, IBallTouchInputProvider
{
    public event Action BallTouched;

    [SerializeField]
    protected float _touchRadius;

    public float TouchRadius { get { return _touchRadius; } }

    private void OnEnable() //subscribe to events
    {
        GameService.Instance.touchInput.TouchDown += OnTouch;
    }

    private void OnDisable() //unsubscribe events
    {
        GameService.Instance.touchInput.TouchDown -= OnTouch;
    }

    private void OnTouch(Vector2 position) //When touch event triggers
    {
        if (Mathf.Abs(Vector2.Distance(transform.position, position)) < TouchRadius) //if touch inside radius
        {
            BallTouched?.Invoke();
        }
    }
}
