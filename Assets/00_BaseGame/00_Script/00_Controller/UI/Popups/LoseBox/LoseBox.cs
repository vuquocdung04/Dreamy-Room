
using UnityEngine.UI;

public class LoseBox : BoxSingleton<LoseBox>
{
    public static LoseBox Setup()
    {
        return Path(PathPrefabs.LOSE_BOX);
    }

    public Button btnRetry;
    public Button btnGoHome;
    public LocalizedText lcTitle;
    public LocalizedText lcRetry;
    public LocalizedText lcNoThanks;
    protected override void Init()
    {
        btnRetry.onClick.AddListener(delegate
        {
            GameController.Instance.ChangeScene2(SceneName.GAME_PLAY);
        });
        btnGoHome.onClick.AddListener(delegate
        {
            GameController.Instance.ChangeScene2(SceneName.HOME_SCENE);
        });
        
        InitLocalization();
    }

    protected override void InitState()
    {
        RefreshLocalization(GameController.Instance.dataContains.DataPlayer, InitLocalization);
    }

    private void InitLocalization()
    {
        lcTitle.Init();
        lcRetry.Init();
        lcNoThanks.Init();
    }
}