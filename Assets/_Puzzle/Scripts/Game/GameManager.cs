using UnityEngine;

public class GameManager : MonoBehaviour
{
    protected LevelManager _levelManager;

    public LevelManager LevelManager { get { return _levelManager; } }

    protected virtual void Awake()
    {
        _levelManager = GetComponentInChildren<LevelManager>();
    }

    protected virtual void OnEnable()
    {
        //subscribe to events
        LevelManager.LoaderPipeFull += OnLoaderPipeFull;
    }
    protected virtual void OnDisable()
    {
        //unsubscribe events
        LevelManager.LoaderPipeFull -= OnLoaderPipeFull;
    }

    protected virtual void Start()
    {
        GameService.Instance.gameManager = this;
    }

    protected virtual void OnLoaderPipeFull() {}
}
