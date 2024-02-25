using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameServiceFactory : MonoBehaviour
{
    ///<summary>
    /// Concrete solution to the issue "what happens if someone clones my game service"
    /// Or even "what happens when all of my game scenes actually need the game service"
    /// GameServiceFactory - spawns an instance of your beloved game manager singleton
    /// No matter how many times you should accidentally clone this script, the singleton will spawn unaltered.
    ///</summary>>

    public GameObject gameServicePrefab;

    public void Awake()
    {
        Instantiate(gameServicePrefab);
    }
}
