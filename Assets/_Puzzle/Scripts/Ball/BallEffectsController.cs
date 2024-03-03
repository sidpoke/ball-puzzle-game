
using UnityEngine;

public class BallEffectsController : MonoBehaviour, IBallEffectsController
{
    private SpriteRenderer spriteRenderer;

    [Header("Shader Behavior")]
    [SerializeField] private float highlightMultiplier = 1f;
    [SerializeField] private float highlightNegativeMultiplier = 2f;
    [Header("VFX Score")]
    [SerializeField] private GameObject vfxScorePrefab;

    private GameObject lastVfxScoreText;

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
        if(lastVfxScoreText != null) { Destroy(lastVfxScoreText.gameObject); }
        lastVfxScoreText = Instantiate(vfxScorePrefab, position, Quaternion.identity) as GameObject;
        lastVfxScoreText.GetComponent<ScoreTextVFX>().SetScoreText(score, color);
    }
}
