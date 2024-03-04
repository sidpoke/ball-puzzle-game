using UnityEngine;

/// <summary>
/// Handles Ball Shader properties and VFX
/// </summary>
public class BallEffectsController : MonoBehaviour, IBallEffectsController
{
    private SpriteRenderer spriteRenderer;

    [Header("Shader Behavior")]
    [SerializeField] private float highlightMultiplier = 1f; //when in the same row but not selected
    [SerializeField] private float highlightNegativeMultiplier = 2f; //when selected
    [Header("VFX Score")]
    [SerializeField] private GameObject vfxScorePrefab;
    [Header("Explosion")]
    [SerializeField] private GameObject explosionPrefab;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// Sets HighlightMultiplier in material properties
    /// </summary>
    public void SetHighlight(bool isBall, bool isInRow) //set highlight shader
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

    /// <summary>
    /// Sets BombTickPhase and BombTickSpeed in material properties (only works if Bomb Material is applied)
    /// </summary>
    public void SetBombTick(float phase, float speed)
    {
        spriteRenderer.material.SetFloat("_BombTickPhase", phase);
        spriteRenderer.material.SetFloat("_BombTickSpeed", speed);
    }

    /// <summary>
    /// Spawns a VFX score text at postion with score value and color
    /// </summary>
    public void SpawnScoreText(Vector2 position, int score, Color color)
    {
        GameObject VfxScoreText = Instantiate(vfxScorePrefab, position, Quaternion.identity) as GameObject;
        VfxScoreText.GetComponent<ScoreTextVFX>().SetScoreText(score, color);
    }

    /// <summary>
    /// Spawns a VFX explosion prefab
    /// </summary>
    public void SpawnExplosion(Vector2 position)
    {
        GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity) as GameObject;
    }
}
