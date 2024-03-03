using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic implementation of an audio controller, which plays sounds from a list
/// </summary>
public class BallAudioHandler: MonoBehaviour, IBallAudioHandler
{
    /// <summary>
    /// Plays a clip that's listed within the BallAudioController's ballAudioClip list
    /// </summary>
    /// <param name="audio"></param>
    public void PlayAudio(string name)
    {
        if(!GameService.Instance) { return; }

        GameService.Instance.audioManager.PlaySound(name);
    }
}
