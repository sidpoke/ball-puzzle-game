using UnityEngine;

public class BallEventHandler : MonoBehaviour, IBallEventHandler
{
    public void BallTouched(BallController ball)
    {
        GameService.Instance.eventManager.Event_BallTouched(ball);
    }

    public void BallScoreAdded(int score)
    {
        GameService.Instance.eventManager.Event_BallScoreAdded(score);
    }
}