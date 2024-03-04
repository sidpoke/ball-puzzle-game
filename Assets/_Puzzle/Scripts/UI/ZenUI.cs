using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles UI Requests for the Zen Mode
/// </summary>
public class ZenUI : MonoBehaviour
{
    public event Action<bool> PauseGame;
    public event Action<string> SwitchScene;

    [Header("UI Panel References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseButton;

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
        PauseGame?.Invoke(state);
    }

    public void CallSwitchToScene(string sceneName) // Called by UI, Return to menu / Reset
    {
        SwitchScene?.Invoke(sceneName);
    }
}
