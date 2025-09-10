
public class GamePlayController : Singleton<GamePlayController>
{
    public GameScene gameScene;
    protected override void OnAwake()
    {
        base.OnAwake();
        m_DontDestroyOnLoad = false;
    }


    private void Init()
    {
        gameScene.Init();
    }
}
