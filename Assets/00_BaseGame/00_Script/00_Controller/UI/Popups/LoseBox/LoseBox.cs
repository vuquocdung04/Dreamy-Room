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
            //NOTE: RETRY
        });
        btnGoHome.onClick.AddListener(delegate
        {
            //NOTE: GO HOME
        });
    }

    protected override void InitState()
    {
    }
}