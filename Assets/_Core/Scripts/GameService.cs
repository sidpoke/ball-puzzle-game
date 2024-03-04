/// <summary>
/// Game Service acts as a service locator, singleton class
/// Any global classes are stored and referenced here
/// </summary>
public class GameService : Singleton<GameService>
{
    public EventManager eventManager;
    public TouchInputHandler touchInput;
    public SceneController scenes;
    public AdventureManager adventure;
    public SaveManager saveManager;
    public AudioManager audioManager;
    //private PhotonService photonService;

    protected override void Awake()
    {
        base.Awake(); // Call base class to init singleton pattern

        eventManager = GetComponentInChildren<EventManager>();
        touchInput = GetComponentInChildren<TouchInputHandler>();
        scenes = GetComponentInChildren<SceneController>();
        adventure = GetComponentInChildren<AdventureManager>();
        saveManager = GetComponentInChildren<SaveManager>();
        audioManager = GetComponentInChildren<AudioManager>();
    }
}
