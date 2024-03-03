using System;
using UnityEngine;
using UnityEngine.UI;

public class ZenUI : MonoBehaviour
{
    public event Action<bool> PauseGame;
    public event Action<string> SwitchScene;

    [Header("UI Panel References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseButton;

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
