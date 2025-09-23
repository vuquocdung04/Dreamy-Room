

using Sirenix.OdinInspector;

public class HomeController : Singleton<HomeController>
{
    public HomeScene homeScene;
    protected override void OnAwake()
    {
        base.OnAwake();
        m_DontDestroyOnLoad = false;
        Init();
    }

    
    
    private void Init()
    {
        homeScene.Init();
    }

    [Button("Button Test")]
    void ButtonTest()
    {
        SettingGameBox.Setup().Show();
    }
}
