
public class GamePlayController : Singleton<GamePlayController>
{
    public GameScene gameScene;
    public PlayerContains playerContains;
    public LevelController levelController;
    public EffectController effectController;
    protected override void OnAwake()
    {
        base.OnAwake();
        m_DontDestroyOnLoad = false;
        Init();
    }
    
    private void Init()
    {
        gameScene.Init();
        levelController.Init();
        playerContains.Init();
        effectController.Init();
    }
    
    public void PauseGame()
    {
        gameScene.PauseTime();   
        playerContains.PauseGame(true);
    }

    public void ResumeGame()
    {
        gameScene.ResumeTime();
        playerContains.PauseGame(false);
    }
}
