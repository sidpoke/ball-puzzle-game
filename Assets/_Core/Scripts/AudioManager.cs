using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores an audio clip with a name identifier, serializable
/// </summary>

[System.Serializable]
public struct GameAudio
{
    public string Name;
    public AudioClip Clip;
}

/// <summary>
/// Audio Manager class plays sounds & music globally
/// </summary>
public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Audio Setup")]
    [SerializeField] private List<GameAudio> audioClips;

    private void Awake() //Get components
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays an audio from the list one shot globally
    /// </summary>
    public void PlaySound(string name)
    {
        AudioClip clip = audioClips?.Find(clip => clip.Name == name).Clip;

        if (audioSource && !string.IsNullOrEmpty(name) && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
