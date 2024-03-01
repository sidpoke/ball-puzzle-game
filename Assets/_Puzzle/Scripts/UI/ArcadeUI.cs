using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search.Providers;
using UnityEngine;
using UnityEngine.UI;

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

    public void SetScoreText(int score)
    {
        scoreText.SetText($"Score: {score.ToString()}");
    }

    public void SetDifficultyText(string difficulty)
    {
        difficultyText.SetText(difficulty);
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
