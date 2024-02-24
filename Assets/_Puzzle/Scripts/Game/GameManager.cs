using UnityEngine;

public class GameManager : MonoBehaviour
{
    protected LevelManager _levelManager;

    public LevelManager LevelManager { get { return _levelManager; } }

    //private UIManager uiManager;
    //private ScoreManager scoreManager;
    //private ISeedProvider seedProvider;

    protected virtual void Awake()
    {
        _levelManager = GetComponentInChildren<LevelManager>();
    }
    protected virtual void Start()
    {
        GameService.Instance.gameManager = this;
    }
}
