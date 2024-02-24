using System;
using UnityEditor;
using UnityEngine;

public class TimerProvider : MonoBehaviour, ITimerProvider
{
    public event Action Timeout;
    public event Action Started;
    public event Action Stopped;

    [SerializeField]
    private float _timerTime;

    private bool _running;
    private float _timer;

    public float CurrentTime { get { return _timer; } }
    public float TimerTime { get { return _timerTime; } }
    public bool IsRunning { get { return _running; } }


    public void Update()
    {
        if (_running)
        {
            _timer += Time.deltaTime;
        }

        if(_timer >= _timerTime)
        {
            Timeout?.Invoke();
            _timer = 0f;
        }
    }

    public void TimerStart()
    {
        _running = true;
        Timeout?.Invoke();
    }

    public void TimerStop() 
    {
        _running = false;
        Timeout?.Invoke();
    }

    public void TimerReset()
    {
        _timer = 0;
    }

    public void SetTimerTime(float time)
    {
        _timerTime = time;
    }
}