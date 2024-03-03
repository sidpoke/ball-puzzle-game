using TMPro;
using UnityEngine;

public class ComboText : MonoBehaviour
{
    [Header("Combo Text Setup")]
    [SerializeField] private string startAnimation;
    [SerializeField] private float destroyTime = 2.0f;

    public void Start()
    {
        GetComponent<Animator>().Play(startAnimation, 0, 0);
        //Play sound too!!
        Destroy(gameObject, destroyTime);
    }
}
