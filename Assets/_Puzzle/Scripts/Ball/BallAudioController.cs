using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores an audio clip with a name identifier, serializable
/// </summary>
[System.Serializable]
public struct BallAudio
{
    public string Name;
    public AudioClip Clip;
}

/// <summary>
/// Basic implementation of an audio controller, which plays sounds from a list
/// </summary>
public class BallAudioController : MonoBehaviour, IBallAudioController
{
    [Header("Audio Setup")]
    [SerializeField] private List<BallAudio> ballAudioClips;
    
    /// <summary>
    /// Plays a clip that's listed within the BallAudioController's ballAudioClip list
    /// </summary>
    /// <param name="audio"></param>
    public void PlayAudio(string audio)
    {
        AudioClip clip = ballAudioClips.Find(x => x.Name == audio).Clip;
        GameService.Instance.audioManager.PlaySound(clip);
    }
}
