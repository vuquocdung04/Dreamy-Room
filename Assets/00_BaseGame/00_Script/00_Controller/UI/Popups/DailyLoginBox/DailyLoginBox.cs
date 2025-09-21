using UnityEngine;
using UnityEngine.UI;

public class DailyLoginBox : BoxSingleton<DailyLoginBox>
{
    public static DailyLoginBox Setup()
    {
        return Path(PathPrefabs.DAILY_LOGIN_BOX); 
    }
    public Button btnClose;
    protected override void Init()
    {
        btnClose.onClick.AddListener(Close);
    }

    protected override void InitState()
    {
    }
}