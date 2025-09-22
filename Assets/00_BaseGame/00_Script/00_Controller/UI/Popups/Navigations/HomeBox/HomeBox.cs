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

    [Header("Daily Login"), Space(5)]
    public Button btnDailylogin;
    public Transform notifyDailyLogin;
    [Header("Daily Reward"), Space(5)]
    public Button btnDailyreward;
    public Transform notifyDailyReward;
    
    protected override void Init()
    {
        btnDailylogin.onClick.AddListener(delegate
        {
            DailyLoginBox.Setup().Show();
        });
        
        btnDailyreward.onClick.AddListener(delegate
        {
            DailyRewardBox.Setup().Show();
        });
        this.RegisterListener(EventID.UPDATE_NOTIFY_DAILYLOGIN,UpdateNotifyDailyLogin);
        UpdateNotifyDailyLogin();
    }

    protected override void InitState()
    {
        
    }

    private void UpdateNotifyDailyLogin(object obj = null)
    {
        var claimed = GameController.Instance.dataContains.dataDaily.HasClaimStreakToday();
        if(claimed) notifyDailyLogin.gameObject.SetActive(false);
        else notifyDailyLogin.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.UPDATE_NOTIFY_DAILYLOGIN,UpdateNotifyDailyLogin);
    }
}