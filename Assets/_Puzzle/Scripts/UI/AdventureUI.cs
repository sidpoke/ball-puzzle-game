using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventureUI : MonoBehaviour
{
    public event Action<bool> PauseGame;
    public event Action<string> SwitchScene;

    [Header("In-Game UI References")]
    [SerializeField] private TMP_Text scoreText;

    [Header("UI Panel References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button pauseButton;

    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text highScoreText;

    public void SetScoreText(int score, int scoreToBeat)
    {
        scoreText.SetText($"{score.ToString()} / {scoreToBeat.ToString()}");
    }

    public void OpenGameOverMenu(int score, int highscore) // Game end menu
    {
        gameOverPanel.SetActive(true);
        pauseButton.gameObject.SetActive(false);

        finalScoreText.SetText($"Your Score:\r\n<b>{score}</b>");
        highScoreText.SetText($"Your Highscore:\r\n<b>{highscore}</b>");
    }

    public void CloseGameOverMenu() // Game end menu
    {
        gameOverPanel.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    public void OpenPauseMenu() // Pause button
    {
        pausePanel.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    public void ClosePauseMenu() // Continue game button
    {
        pausePanel.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    public void CallPauseGame(bool state)
    {
        PauseGame?.Invoke(state);
    }

    public void CallSwitchToScene(string sceneName) // Return to menu
    {
        SwitchScene?.Invoke(sceneName);
    }
}
