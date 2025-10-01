
public class GamePlayController : Singleton<GamePlayController>
{
    public GameScene gameScene;
    public PlayerContains playerContains;
    protected override void OnAwake()
    {
        base.OnAwake();
        m_DontDestroyOnLoad = false;
        Init();
    }
    
    private void Init()
    {
        gameScene.Init();
        playerContains.Init();
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
