using UnityEngine;

public class GameManager : MonoBehaviour
{
    protected LevelManager _levelManager;

    public LevelManager LevelManager { get { return _levelManager; } }

    protected virtual void Awake()
    {
        _levelManager = GetComponentInChildren<LevelManager>();
    }

    protected virtual void Start()
    {
        //Sets service instance here
        GameService.Instance.gameManager = this;
    }
}
