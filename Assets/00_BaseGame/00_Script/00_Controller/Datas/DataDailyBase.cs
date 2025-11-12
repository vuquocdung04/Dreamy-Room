using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DataDailySO", menuName = "DATA/DataDaily", order = 0)]
public class DataDailyBase : ScriptableObject
{
    // ===== CHỈ LƯU CONFIG (không lưu runtime data) =====
    [SerializeField] private int totalDaysInCycle = 6;
    
    [Header("Conflic Gift Of The Week")]
    [SerializeField] private List<DataDailyReward> streakRewards;

    public string KeyClaim;
    public string KeyClaimed;
    public string KeyFree;
    [SerializeField] private DataDailyReward freeDailyReward;
    [SerializeField] private List<DataDailyReward> adRewardsList;
    
    private DataPlayer PlayerData => GameController.Instance.dataContains.DataPlayer;
    public int GetStreakDayIndex() => PlayerData.playerClaimedDay;
    public bool HasClaimStreakToday() => PlayerData.hasClaimedStreakToday;
    
    public void HandleStreakClaimed()
    {
        var reward = streakRewards[PlayerData.playerClaimedDay];
        GameController.Instance.dataContains.giftData.Claim(reward.giftType, reward.amount);
        Debug.Log("Đã nhận quà STREAK Ngày: " + (PlayerData.playerClaimedDay + 1));
        
        PlayerData.playerClaimedDay++;
        PlayerData.hasClaimedStreakToday = true;
    }

    // ===== TIERED LOGIC =====
    public bool IsFreeClaimedToday() => PlayerData.isFreeClaimedToday;

    public void ClaimFreeReward()
    {
        GameController.Instance.dataContains.giftData.Claim(freeDailyReward.giftType, freeDailyReward.amount);
        Debug.Log($"Đã nhận quà FREE hàng ngày: {freeDailyReward.giftType} - {freeDailyReward.amount}");
        PlayerData.isFreeClaimedToday = true;
    }

    public int GetAdRewardsClaimedCount() => PlayerData.adRewardsClaimedCount;
    public bool AllAdRewardsClaimed() => PlayerData.adRewardsClaimedCount >= adRewardsList.Count;

    public DataDailyReward GetNextAdRewardInfo()
    {
        if (AllAdRewardsClaimed()) return null;
        return adRewardsList[PlayerData.adRewardsClaimedCount];
    }

    public void ClaimNextAdReward()
    {
        var reward = adRewardsList[PlayerData.adRewardsClaimedCount];
        GameController.Instance.dataContains.giftData.Claim(reward.giftType, reward.amount);
        PlayerData.adRewardsClaimedCount++;
    }
    
    public void PrepareForNewDay()
    {
        // Kiểm tra và reset chu kỳ nếu cần
        if (PlayerData.playerClaimedDay >= totalDaysInCycle)
        {
            PlayerData.playerClaimedDay = 0;
        }
        // Luôn cho phép nhận quà vào ngày mới
        PlayerData.hasClaimedStreakToday = false;
        PlayerData.isFreeClaimedToday = false;
        PlayerData.adRewardsClaimedCount = 0;
    }
}

[System.Serializable]
public class DataDailyReward
{
    [HorizontalGroup("RewardData", Width = 70)]
    [LabelWidth(30)]
    public int day = 1;
    [HorizontalGroup("RewardData", Width = 90)] 
    [LabelWidth(50)] 
    public int amount;
    [HorizontalGroup("RewardData")] 
    public GiftType giftType;
}