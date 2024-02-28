using TMPro;
using UnityEngine;

public class GameMode_Arcade : GameManager
{
    private ITimerProvider timer;
    private IScoreManager scoreManager;
    //private Arcade UIManager  :)))

    [SerializeField]
    private TMP_Text scoreText;
    
    protected override void Awake()
    {
        base.Awake();
        timer = GetComponent<ITimerProvider>();
        scoreManager = GetComponent<IScoreManager>();

        scoreManager.ScoreChanged += OnScoreChanged;
        timer.Timeout += OnTimerTimeout;
    }

    protected override void Start()
    {
        base.Start();
        LevelManager.FillAllPipes();
        timer.TimerStart();
    }

    // Update is called once per frame
    private void Update()
    {
        //Cheats
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LevelManager.AddBallToPipe(LevelManager.LoaderPipe);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            LevelManager.LoaderPipe.PipeStorage.Release();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            int random = Random.Range(0, LevelManager.LoaderPipe.PipeStorage.Balls.Count);
            LevelManager.LoaderPipe.PipeStorage.RemoveAt(random);
        }
    }

    public void OnScoreChanged(int score)
    {
        scoreText.SetText($"Score: {score.ToString()}");
    }

    private void OnTimerTimeout()
    {
        LevelManager.AddBallToPipe(LevelManager.LoaderPipe);
    }
}
