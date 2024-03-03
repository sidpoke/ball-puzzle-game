using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class TimerProvider : MonoBehaviour, ITimerProvider
{
    public event Action Timeout;
    public event Action Started;
    public event Action Stopped;

    [Header("Debug")]
    [SerializeField] private float _timerTime;
    [SerializeField] private bool _running;
    [SerializeField] private float _timer;

    public float CurrentTime { get { return _timer; } }
    public float TimerTime { get { return _timerTime; } }
    public bool IsRunning { get { return _running; } }

    public void Awake()
    {
        TimerReset();
    }

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
        Started?.Invoke();
    }

    public void TimerStop() 
    {
        _running = false;
        Stopped?.Invoke();
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