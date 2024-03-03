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

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Audio Setup")]
    [SerializeField] private List<GameAudio> audioClips;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string name)
    {
        if(audioSource)
        {
            audioSource.PlayOneShot(audioClips.Find(clip => clip.Name == name).Clip);
        }
    }
}
