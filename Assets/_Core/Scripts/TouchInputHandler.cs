using System;
using UnityEngine;

/// <summary>
/// Fires events once mouse click or touch events occured.
/// TouchDown fires Vector2 position in world space as event once clicked or touched.
/// </summary>
public class TouchInputHandler : MonoBehaviour
{
    public event Action<Vector2> TouchDown;
    public event Action TouchReleased;

    private bool pressed;

    void Update()
    {
        //mobile inputs
        if (Input.touchCount > 0 && !pressed)
        {
            TouchDown?.Invoke(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));
            pressed = true;
        }
        if (Input.touchCount == 0 && pressed)
        {
            TouchReleased?.Invoke();
            pressed = false;
        }

        //PC for testing
        if (Input.GetMouseButtonDown(0) && !pressed)
        {
            TouchDown?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            pressed = true;
        }
        if (Input.GetMouseButtonUp(0) && pressed)
        {
            TouchReleased?.Invoke();
            pressed = false;
        }
    }
}
