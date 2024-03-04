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
    [SerializeField] private TMP_Text ballSwapsText;

    [Header("UI Panel References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button pauseButton;

    [SerializeField] private TMP_Text scoreTitleText;
    [SerializeField] private TMP_Text finalScoreText;

    public void SetScoreText(int score, int scoreToBeat)
    {
        scoreText.SetText($"{score.ToString()} / {scoreToBeat.ToString()}");
    }

    public void SetBallSwapTriesText(int tries)
    {
        ballSwapsText.SetText($"{tries.ToString()} Moves");
    }

    public void OpenGameOverMenu(int score, int scoreToBeat) // Game end menu
    {
        gameOverPanel.SetActive(true);
        pauseButton.gameObject.SetActive(false);

        finalScoreText.SetText($"Score:\r\n<b>{score} out of {scoreToBeat}</b>");
        scoreTitleText.SetText(score >= scoreToBeat ? "YOU WIN!" : "FAILED");
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
