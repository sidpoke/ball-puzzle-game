using System;

public interface IScoreManager
{
    public int Score { get; }
    public void AddScore(int points);
}
