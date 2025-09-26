using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DataDailySO", menuName = "DATA/DataDaily", order = 0)]
public class DataDailyBase : ScriptableObject
{
    
    /// <summary>
    /// Bien va logic Daily login
    /// </summary>
    
    [SerializeField] private int playerClaimedDay;
    [SerializeField] private int totalDaysInCycle = 6;
    [SerializeField] private bool hasClaimedStreakToday;
    [Header("Conflic Gift Of The Week")]
    [SerializeField] List<DataDailyReward> streakRewards;
    public int GetStreakDayIndex() => playerClaimedDay;
    public bool HasClaimStreakToday() => hasClaimedStreakToday;
    public void HandleStreakClaimed()
    {
        var reward = streakRewards[playerClaimedDay];
        GameController.Instance.dataContains.giftData.Claim(reward.giftType,reward.amount);
        Debug.Log("Đã nhận quà STREAK Ngày: " + (playerClaimedDay + 1));
        playerClaimedDay++;
        hasClaimedStreakToday = true;
    }

    /// <summary>
    /// Bien va Logic che do tiered
    /// </summary>
    [Header("Conflic Gift Of The Day"), Space(15)]
    [SerializeField] private bool isFreeClaimedToday;
    [SerializeField] private DataDailyReward freeDailyReward;
    [SerializeField] private int adRewardsClaimedCount;
    [SerializeField] private List<DataDailyReward> adRewardsList;
    
    public bool IsFreeClaimedToday() => isFreeClaimedToday;

    public void ClaimFreeReward()
    {
        GameController.Instance.dataContains.giftData.Claim(freeDailyReward.giftType,freeDailyReward.amount);
        Debug.Log($"Đã nhận quà FREE hàng ngày: {freeDailyReward.giftType} - {freeDailyReward.amount}");
        isFreeClaimedToday = true;
    }
    public int GetAdRewardsClaimedCount() => adRewardsClaimedCount;
    public bool AllAdRewardsClaimed() =>adRewardsClaimedCount >= adRewardsList.Count;

    public DataDailyReward GetNextAdRewardInfo()
    {
        if (AllAdRewardsClaimed()) return null;
        return adRewardsList[adRewardsClaimedCount];
    }

    public void ClaimNextAdReward()
    {
        var reward = adRewardsList[adRewardsClaimedCount];
        GameController.Instance.dataContains.giftData.Claim(reward.giftType,reward.amount);
        adRewardsClaimedCount++;
    }
    
    public void PrepareForNewDay()
    {
        // 1. Kiểm tra và reset chu kỳ nếu cần
        if (playerClaimedDay >= totalDaysInCycle)
        {
            playerClaimedDay = 0;
        }
        // 2. Luôn cho phép nhận quà vào ngày mới
        hasClaimedStreakToday = false;
        isFreeClaimedToday  = false;
        adRewardsClaimedCount = 0;
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