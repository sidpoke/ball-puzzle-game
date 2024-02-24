using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallColor
{
    Red,
    Green, 
    Blue,
    Yellow,
    Any
}

public class Ball_Regular : BallController
{
    [SerializeField]
    private BallColor _color;

    protected override void Awake()
    {
        base.Awake();
        touchInputProvider.OnTouchResponse += OnTouch;
    }

    void OnTouch()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
