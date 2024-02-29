using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BallEffectsHandler : MonoBehaviour, IBallEffectsHandler
{
    private SpriteRenderer spriteRenderer;

    [Header("Shader Behavior")]
    [SerializeField]
    private float highlightMultiplier = 1f;
    [SerializeField]
    private float highlightNegativeMultiplier = 2f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

}
