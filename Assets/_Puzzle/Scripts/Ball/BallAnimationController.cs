using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Basic implementation of an animation controller, which plays animations using an Animator
/// </summary>
public class BallAnimationController : MonoBehaviour, IBallAnimationController
{
    [Header("Audio Setup")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Plays a clip that's listed within the BallAudioController's ballAudioClip list
    /// </summary>
    /// <param name="audio">The clip identifier</param>
    public void PlayAnimation(string name)
    {
        animator.Play(name, 0, 0);
    }
}
