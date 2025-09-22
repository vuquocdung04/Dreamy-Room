using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DataDailySO", menuName = "DATA/DataDaily", order = 0)]
public class DataDailySO : ScriptableObject
{
    [SerializeField] private int playerClaimedDay;
    [SerializeField] private int totalDaysInCycle = 6;
    [SerializeField] private bool hasClaimedDay;
    [Header("gift"), Space(5)]
    [SerializeField] List<DataDailyReward> lsRewards;
    
    public int GetClaimedDay() => playerClaimedDay;
    public bool HasClaimedDay() => hasClaimedDay;
    public void HandleClaimed()
    {
        Claim();
        playerClaimedDay++;
        hasClaimedDay = true;
    }
    
    public void PrepareForNewDay()
    {
        // 1. Kiểm tra và reset chu kỳ nếu cần
        if (playerClaimedDay >= totalDaysInCycle)
        {
            playerClaimedDay = 0;
        }
        // 2. Luôn cho phép nhận quà vào ngày mới
        hasClaimedDay = false;
    }

    private void Claim()
    {
        var reward = lsRewards[playerClaimedDay];
        GameController.Instance.dataContains.giftData.Claim(reward.giftType,reward.amount);
        Debug.Log("Da nhan qua ngay: " + playerClaimedDay +" " + reward.giftType + " " +  reward.amount);
    }
    
    [Button("ReDay", ButtonSizes.Large)]
    void ReDay()
    {
        UseProfile.FirstTimeOpenGame = TimeManager.GetCurrentTime().AddDays(-1);
    }
}

[System.Serializable]
public class DataDailyReward
{
    [HorizontalGroup("RewardData", Width = 50)]
    [LabelWidth(30)]
    public int day = 1;
    [HorizontalGroup("RewardData", Width = 90)] 
    [LabelWidth(50)] 
    public int amount;
    [HorizontalGroup("RewardData")] 
    public GiftType giftType;
}