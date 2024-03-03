using UnityEngine;

public interface IBallEffectsController
{
    public void SetHighlight(bool isBall, bool isInRow);
    public void SpawnScoreText(Vector2 position, int score, Color color);
    public void SpawnExplosion(Vector2 position);
}
