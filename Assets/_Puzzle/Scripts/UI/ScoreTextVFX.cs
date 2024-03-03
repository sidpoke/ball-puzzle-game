using TMPro;
using UnityEngine;

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
