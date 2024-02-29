using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BallInputProvider : MonoBehaviour, IBallTouchInputProvider
{
    public event Action BallTouched;

    [SerializeField]
    protected float _touchRadius;

    public float TouchRadius { get { return _touchRadius; } }

    private void Start()
    {
        if (GameService.Instance != null)
        {
            GameService.Instance.touchInput.TouchDown += OnTouch;
        }
    }

    private void OnDestroy()
    {
        if (GameService.Instance != null)
        {
            GameService.Instance.touchInput.TouchDown -= OnTouch;
        }
    }

    private void OnTouch(Vector2 position)
    {
        if (Mathf.Abs(Vector2.Distance(transform.position, position)) < TouchRadius)
        {
            BallTouched?.Invoke();
        }
    }
}
