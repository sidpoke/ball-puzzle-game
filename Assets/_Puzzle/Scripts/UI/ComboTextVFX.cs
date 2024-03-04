using UnityEngine;

/// <summary>
/// A VFX text that spawns after a combo executed
/// </summary>
public class ComboTextVFX : MonoBehaviour
{
    [Header("Combo Text Setup")]
    [SerializeField] private string startAnimation;
    [SerializeField] private string comboAudioName;
    [SerializeField] private float destroyTime = 2.0f;

    public void Start()
    {
        GetComponent<Animator>().Play(startAnimation, 0, 0);
        if (!string.IsNullOrEmpty(comboAudioName))
        {
            GameService.Instance.audioManager.PlaySound(comboAudioName);
        }
        Destroy(gameObject, destroyTime);
    }
}
