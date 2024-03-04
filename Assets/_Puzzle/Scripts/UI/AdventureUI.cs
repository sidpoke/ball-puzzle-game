using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles UI Requests for the Adventure Mode
/// </summary>
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

    public void SetScoreText(int score, int scoreToBeat) // Called by game manager
    {
        scoreText.SetText($"{score.ToString()} / {scoreToBeat.ToString()}");
    }

    public void SetBallSwapTriesText(int tries) // Called by game manager
    {
        ballSwapsText.SetText($"{tries.ToString()} Moves");
    }

    public void OpenGameOverMenu(int score, int scoreToBeat) // Called by game manager
    {
        gameOverPanel.SetActive(true);
        pauseButton.gameObject.SetActive(false);

        finalScoreText.SetText($"Score:\r\n<b>{score} out of {scoreToBeat}</b>");
        scoreTitleText.SetText(score >= scoreToBeat ? "YOU WIN!" : "FAILED");
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
