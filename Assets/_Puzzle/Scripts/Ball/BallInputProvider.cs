using System;
using UnityEngine;

public class BallInputProvider : MonoBehaviour, IBallTouchInputProvider
{
    [SerializeField]
    protected float _touchRadius;

    public float TouchRadius { get { return _touchRadius; } }

    public event Action OnTouchResponse;

    private void Update()
    {
        //mobile inputs
        if (Input.touchCount > 0) 
        {
            Touch(Input.GetTouch(0).position);
        }
        //PC testing
        if (Input.GetMouseButtonDown(0))
        {
            Touch(Input.mousePosition);
        }
    }

    private void Touch(Vector2 position)
    {
        var pos = Camera.main.ScreenToWorldPoint(position);
        if (Mathf.Abs(Vector2.Distance(transform.position, pos)) < TouchRadius)
        {
            OnTouchResponse?.Invoke();
        }
    }
}
