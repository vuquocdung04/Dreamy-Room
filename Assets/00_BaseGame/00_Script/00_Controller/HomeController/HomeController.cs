

public class HomeController : Singleton<HomeController>
{
    public HomeScene homeScene;
    protected override void OnAwake()
    {
        base.OnAwake();
        m_DontDestroyOnLoad = false;
    }

    private void Init()
    {
        homeScene.Init();
    }
}
