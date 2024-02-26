using UnityEngine;

public class BallEventHandler : MonoBehaviour, IBallEventHandler
{
    public void EventBallTouched(BallController ball)
    {
        GameService.Instance.eventManager.EventBallTouched(ball);
    }
}