using UnityEngine;
using UnityEngine.UI;

public class LoseBox : BoxSingleton<LoseBox>
{
    public static LoseBox Setup()
    {
        return Path(PathPrefabs.LOSE_BOX);
    }

    public Button btnRetry;
    public Button btnGoHome;
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
    }

    protected override void InitState()
    {
    }
}