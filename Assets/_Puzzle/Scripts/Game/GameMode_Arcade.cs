using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode_Arcade : GameManager
{
    protected override void Awake()
    {
        base.Awake();
    }

    // Arcade game logic i suppose
    protected override void Start()
    {
        base.Start();
        LevelManager.FillAllPipes();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LevelManager.AddBallToLoader();
        }
    }
}
