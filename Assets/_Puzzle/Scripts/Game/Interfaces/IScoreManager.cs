using System;

public interface IScoreManager
{
    public event Action<int> ScoreChanged; 
    public int Score { get; }
    public void AddScore(int points);

    public void ResetScore();
}
