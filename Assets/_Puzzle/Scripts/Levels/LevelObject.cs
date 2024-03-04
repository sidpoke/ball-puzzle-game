using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic set of Balls used in a level in Adventure Mode
/// Can be extended if more ball types are needed
/// </summary>
public enum LevelBalls //align with levelmanager objects for best experience
{
    Ball_Red = 0,
    Ball_Green = 1,
    Ball_Blue = 2,
    Ball_Yellow = 3,
    Ball_Any = 4,
    Ball_Block = 5,
    Ball_NoSwitch = 6,
    Ball_Bomb = 7,
    Ball_Laser = 8
}

/// <summary>
/// Level Object used by Adventure Mode, contains simple instructions on how the level is set up
/// </summary>
[CreateAssetMenu]
public class LevelObject : ScriptableObject
{
    public string LevelName;
    public int ScoreToBeat = 0;
    public int AmountOfSwitches = 0;
    public List<LevelBalls> BallsRedPipe;
    public List<LevelBalls> BallsBluePipe;
    public List<LevelBalls> BallsGreenPipe;
    public List<LevelBalls> BallsYellowPipe;
}
