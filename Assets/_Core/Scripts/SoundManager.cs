using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
