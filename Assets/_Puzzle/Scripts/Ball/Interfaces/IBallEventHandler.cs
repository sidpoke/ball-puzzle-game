using UnityEngine;

public interface IBallEventHandler
{
    //Events that a ball triggers
    public void BallTouched(BallController ball);
    public void BallScoreAdded(int score);
}