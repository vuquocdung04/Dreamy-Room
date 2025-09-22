using System;
using DG.Tweening;
using EventDispatcher;
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
    public Transform notifyDailyLogin;
    
    protected override void Init()
    {
        btnDailylogin.onClick.AddListener(delegate
        {
            DailyLoginBox.Setup().Show();
        });
        this.RegisterListener(EventID.UPDATE_NOTIFY_DAILYLOGIN,UpdateNotifyDailyLogin);
        UpdateNotifyDailyLogin();
    }

    protected override void InitState()
    {
        
    }

    private void UpdateNotifyDailyLogin(object obj = null)
    {
        var claimed = GameController.Instance.dataContains.dataDaily.HasClaimedDay();
        if(claimed) notifyDailyLogin.gameObject.SetActive(false);
        else notifyDailyLogin.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.UPDATE_NOTIFY_DAILYLOGIN,UpdateNotifyDailyLogin);
    }
}