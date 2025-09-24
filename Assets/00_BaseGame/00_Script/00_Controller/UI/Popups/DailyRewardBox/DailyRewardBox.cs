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
    public Image imageFreeBtn;
    public Sprite claimedSprite;
    public Sprite adclaimableSprite;
    public List<DailyRewardItem> adRewardItems;
    
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
        for (int i = 0; i < adRewardItems.Count; i++)
        {
            var item = adRewardItems[i];
            if (i < adsClaimedCount)
            {
                item.SetAsClaimed();
                item.UpdateImageBtn(claimedSprite);
            }
            else if (i == adsClaimedCount)
            {
                item.SetAsClaimable();
                item.UpdateImageBtn(adclaimableSprite);
            }
            else
            {
                item.InActiveBtn();
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