using System;

public interface ITimerProvider
{
    public event Action Timeout;
    public event Action Started;
    public event Action Stopped;

    public float CurrentTime { get; }
    public float TimerTime { get; }
    public bool IsRunning { get; }

    public void SetTimerTime(float time);
    public void TimerStart();
    public void TimerStop();
    public void TimerReset();
}
