
using EventDispatcher;
using TMPro;
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
    [Header("EditProfile"), Space(5)]
    public Button btnEditProfile;
    [Header("Setting")]
    public Button btnSetting;
    [Header("RemoveAds")]
    public Button btnRemoveAds;
    [Header("StartPack")]
    public Button btnStartPack;
    [Header("Treasure")]
    public Button btnTreasure;
    public TextMeshProUGUI txtTreasure;
    public Image fillTreasure;
    [Header("PigBank")]
    public Button btnPigBank;
    public TextMeshProUGUI txtPigBank;
    public Image fillPigBank;
    
    
    protected override void Init()
    {
        SetupBtnOnClick(btnDailylogin, () => DailyLoginBox.Setup().Show());
        SetupBtnOnClick(btnDailyreward, () => DailyRewardBox.Setup().Show());
        SetupBtnOnClick(btnEditProfile, () => EditProfileBox.Setup().Show());
        SetupBtnOnClick(btnSetting, () => SettingHomeBox.Setup().Show());
        SetupBtnOnClick(btnRemoveAds, () => RemoveAdsBox.Setup().Show());
        SetupBtnOnClick(btnPigBank, () => PigBankBox.Setup().Show());
        SetupBtnOnClick(btnTreasure, delegate
        {
            TreasureBox.Setup().Show();
        });
        SetupBtnOnClick(btnStartPack, delegate
        {
            PigBankBox.Setup().Show();
        });
        
        
        this.RegisterListener(EventID.UPDATE_NOTIFY_DAILYLOGIN,UpdateNotifyDailyLogin);
        UpdateNotifyDailyLogin();
        UpdateProgressTreasure();
    }

    protected override void InitState()
    {
        
    }

    private void SetupBtnOnClick(Button btn, System.Action callback = null)
    {
        btn.onClick.AddListener(delegate
        {
            callback?.Invoke();
        });
    }

    private void UpdateProgressTreasure()
    {
        var star = UseProfile.Star;
        var progress = (float)star / 400;
        fillTreasure.fillAmount = progress;
        txtTreasure.text = star.ToString();
    }
    
    
    private void UpdateNotifyDailyLogin(object obj = null)
    {
        var claimed = GameController.Instance.dataContains.dataDaily.HasClaimStreakToday();
        if(claimed) notifyDailyLogin.gameObject.SetActive(false);
        else notifyDailyLogin.gameObject.SetActive(true);
    }

    private void UpdateNotifyDailyReward(object obj = null)
    {
        var allAdRewardsClaimed = GameController.Instance.dataContains.dataDaily.AllAdRewardsClaimed();
        if(allAdRewardsClaimed)  notifyDailyReward.gameObject.SetActive(false);
        else notifyDailyReward.gameObject.SetActive(true);
    }
    
    private void OnDestroy()
    {
        this.RemoveListener(EventID.UPDATE_NOTIFY_DAILYLOGIN,UpdateNotifyDailyLogin);
    }
}