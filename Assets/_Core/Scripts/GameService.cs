using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Service Locator Pattern, Singleton
public class GameService : Singleton<GameService>
{
    public EventManager eventManager;
    public TouchInputHandler touchInput;
    public SceneController scenes;
    public SaveManager saveManager;
    public SoundManager soundManager;
    //private PhotonService photonService;
    //public MenuManager menuManager; //(this will simply set its own instance once loaded)

    protected override void Awake()
    {
        base.Awake(); // Call base class to init singleton pattern

        eventManager = GetComponentInChildren<EventManager>();
        touchInput = GetComponentInChildren<TouchInputHandler>();
        scenes = GetComponentInChildren<SceneController>();
        saveManager = GetComponentInChildren<SaveManager>();
        soundManager = GetComponentInChildren<SoundManager>();
    }
}
