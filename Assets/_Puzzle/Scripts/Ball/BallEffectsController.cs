using UnityEngine;

public class BallEffectsController : MonoBehaviour, IBallEffectsController
{
    private SpriteRenderer spriteRenderer;

    [Header("Shader Behavior")]
    [SerializeField] private float highlightMultiplier = 1f;
    [SerializeField] private float highlightNegativeMultiplier = 2f;
    [Header("VFX Score")]
    [SerializeField] private GameObject vfxScorePrefab;
    [Header("Explosion")]
    [SerializeField] private GameObject explosionPrefab;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetHighlight(bool isBall, bool isInRow)
    {
        if (isBall || isInRow)
        {
            spriteRenderer.material.SetFloat("_HighlightMultiplier", 
                isBall ? -highlightNegativeMultiplier  : isInRow ? highlightMultiplier: 0f);
        }
        else
        {
            spriteRenderer.material.SetFloat("_HighlightMultiplier", 0f);
        }
    }

    public void SpawnScoreText(Vector2 position, int score, Color color)
    {
        GameObject VfxScoreText = Instantiate(vfxScorePrefab, position, Quaternion.identity) as GameObject;
        VfxScoreText.GetComponent<ScoreTextVFX>().SetScoreText(score, color);
    }

    public void SpawnExplosion(Vector2 position)
    {
        GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity) as GameObject;
    }
}
