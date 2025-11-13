using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardBox : BoxSingleton<DailyRewardBox>
{
    public static DailyRewardBox Setup()
    {
        return Path(PathPrefabs.DAILY_REWARD_BOX);
    }

    public Button btnClose;
    public Button btnFreeReward;
    public Sprite claimedSprite;
    public Sprite adclaimableSprite;
    public List<DailyRewardItem> adRewardItems;
    public LocalizedText lcFreeBtn;
    public LocalizedText title;
    protected override void Init()
    {
        UpdateState();
        btnClose.onClick.AddListener(Close);
        btnFreeReward.onClick.AddListener(OnFreeClaim);
        for (int i = 0; i < adRewardItems.Count; i++)
        {
            var item = adRewardItems[i];
            // Gọi hàm Init của item, và truyền vào hành động OnAdRewardClaim
            item.AddClickListener(OnAdRewardClaim);
        }
    }
    protected override void InitState()
    {
        UpdateState();
    }
    private void OnAdRewardClaim()
    {
        GameController.Instance.dataContains.dataDaily.ClaimNextAdReward();
        UpdateState();
    }

    private void UpdateState()
    {
        UpdateFreeRewardBtnState();
        UpdateAdRewardsState();
    }
    private void UpdateAdRewardsState()
    {
        var adsClaimedCount = GameController.Instance.dataContains.dataDaily.GetAdRewardsClaimedCount();
        var dataDaily = GameController.Instance.dataContains.dataDaily;
        title.Init();
        for (int i = 0; i < adRewardItems.Count; i++)
        {
            var item = adRewardItems[i];
            
            if (i < adsClaimedCount)
            {
                // Đã claimed
                item.SetAsClaimed();
                item.UpdateImageBtn(claimedSprite);
                item.UpdateLocalization(dataDaily.KeyClaimed);
            }
            else if (i == adsClaimedCount)
            {
                // Đang có thể claim
                item.SetAsClaimable();
                item.UpdateImageBtn(adclaimableSprite);
                item.UpdateLocalization(dataDaily.KeyClaim);
            }
            else
            {
                // Chưa tới lượt (Free)
                item.SetAsFree();
                item.UpdateLocalization(dataDaily.KeyFree);
            }
        }
    }

    private void OnFreeClaim()
    {
        GameController.Instance.dataContains.dataDaily.ClaimFreeReward();
        UpdateFreeRewardBtnState();
    }
    
    private void UpdateFreeRewardBtnState()
    {
        bool hasClaimed  = GameController.Instance.dataContains.dataDaily.IsFreeClaimedToday();
        btnFreeReward.enabled = !hasClaimed;
        if (hasClaimed)
        {
            btnFreeReward.image.sprite = claimedSprite;
        }
        UpdateFreeRewardBtnText();
    }
    private void UpdateFreeRewardBtnText()
    {
        var dataDaily = GameController.Instance.dataContains.dataDaily;
        bool hasClaimed = dataDaily.IsFreeClaimedToday();
        
        // Nếu đã claimed thì hiện "Claimed", chưa thì hiện "Claim"
        lcFreeBtn.Init(hasClaimed ? dataDaily.KeyClaimed : dataDaily.KeyClaim);
    }
    [Button("Setup Item", ButtonSizes.Large)]
    void SetupItem()
    {
        foreach (var item in adRewardItems)
        {
            item.SetupOdin();
        }
    }
    
}