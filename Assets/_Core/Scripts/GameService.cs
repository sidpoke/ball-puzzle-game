using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Service Locator Pattern, Singleton
public class GameService : Singleton<GameService>
{
    public EventManager eventManager;
    public SceneManager sceneManager;
    public SaveManager saveManager;
    public SoundManager soundManager;
    public PauseMenu pauseMenu;
    //private PhotonService photonService;

    public GameManager gameManager; //(this will simply set its own instance once loaded)
    //public MenuManager menuManager; //(this will simply set its own instance once loaded)

    protected override void Awake()
    {
        base.Awake(); // Call base class to init singleton pattern

        eventManager = GetComponentInChildren<EventManager>();
        sceneManager = GetComponentInChildren<SceneManager>();
        saveManager = GetComponentInChildren<SaveManager>();
        soundManager = GetComponentInChildren<SoundManager>();
        pauseMenu = GetComponentInChildren<PauseMenu>();
    }
}
