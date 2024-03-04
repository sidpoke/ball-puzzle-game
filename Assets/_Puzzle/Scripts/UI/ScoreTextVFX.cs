using TMPro;
using UnityEngine;

/// <summary>
/// A VFX text that spawns after a ball was destroyed to show the score achieved
/// </summary>
public class ScoreTextVFX : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float destroyTime = 2.0f;

    public void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    public void SetScoreText(int score, Color color)
    {
        scoreText.SetText(score.ToString());
        scoreText.color = color;
    }
}
