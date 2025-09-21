using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class HomeBox : BoxSingleton<HomeBox>
{
    public static HomeBox Setup()
    {
        return Path(PathPrefabs.HOME_BOX);
    }

    [Space(10)]
    public Button btnDailylogin;
    
    protected override void Init()
    {
        btnDailylogin.onClick.AddListener(delegate
        {
            DailyLoginBox.Setup().Show();
        });
    }

    protected override void InitState()
    {
        
    }
}