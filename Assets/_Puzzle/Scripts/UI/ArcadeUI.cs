using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles UI Requests for the Arcade Mode
/// </summary>
public class ArcadeUI : MonoBehaviour
{
    public event Action<bool> PauseGame;
    public event Action<string> SwitchScene;

    [Header("In-Game UI References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text difficultyText;

    [Header("UI Panel References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button pauseButton;

    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text highScoreText;

    public void SetScoreText(int score) // Called by game manager
    {
        scoreText.SetText(score.ToString());
    }

    public void SetDifficultyText(string difficulty) // Called by game manager
    {
        difficultyText.SetText(difficulty);
    }

    public void OpenGameOverMenu(int score, int highscore) // Called by game manager
    {
        gameOverPanel.SetActive(true);
        pauseButton.gameObject.SetActive(false);

        finalScoreText.SetText($"Your Score:\r\n<b>{score}</b>");
        highScoreText.SetText($"Your Highscore:\r\n<b>{highscore}</b>");
    }

    public void CloseGameOverMenu() // Called by game manager
    {
        gameOverPanel.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    public void OpenPauseMenu() // Called by game manager
    {
        pausePanel.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    public void ClosePauseMenu() // Called by game manager
    {
        pausePanel.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    public void CallPauseGame(bool state) // Called by UI, pause/unpause game
    {
        GameService.Instance.audioManager.PlaySound("UIButton");
        PauseGame?.Invoke(state);
    }

    public void CallSwitchToScene(string sceneName) // Called by UI, Return to menu / Reset
    {
        GameService.Instance.audioManager.PlaySound("UIButton");
        SwitchScene?.Invoke(sceneName);
    }
}
