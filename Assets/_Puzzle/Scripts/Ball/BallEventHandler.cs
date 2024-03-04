using UnityEngine;

/// <summary>
/// Used to send out events from the BallController globally
/// </summary>
public class BallEventHandler : MonoBehaviour, IBallEventHandler
{
    public void BallTouched(BallController ball) //used eg. by LevelInputProvider
    {
        GameService.Instance.eventManager.Event_BallTouched(ball);
    }

    public void BallScoreAdded(int score) //used eg. by score manager
    {
        GameService.Instance.eventManager.Event_BallScoreAdded(score);
    }
}