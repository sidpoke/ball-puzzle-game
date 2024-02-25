using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBallTouchInputProvider
{
    public float TouchRadius { get; }

    public event Action BallTouched;
}
