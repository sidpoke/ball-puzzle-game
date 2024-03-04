using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Name is an identifier for the code to recognize the scene. Scene is the actual scene to load.
/// This is useful because scene names can change without the code needing adjustment.
/// </summary>
[System.Serializable]
public struct GameScene
{
    public string Name;
    public string Scene;
}

/// <summary>
/// Keeps track of the scenes in the game and switches them using identifiers
/// </summary>
public class SceneController : MonoBehaviour
{
    [SerializeField] private List<GameScene> levels = new List<GameScene>();

    /// <summary>
    /// Loads a scene using its with the "Name" identifier
    /// </summary>
    public void LoadScene(string sceneIdentifier)
    {
        if (levels.Find(level => level.Name == sceneIdentifier) is GameScene scene)
        {
            SceneManager.LoadScene(scene.Scene);
        }
        else
        {
            Debug.LogError($"Scene identifier \"{sceneIdentifier}\" could not be found.");
        }
    }
}
