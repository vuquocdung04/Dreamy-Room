using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "DATA/DATA PLAYER", order = 0)]
public class DataPlayer : ScriptableObject
{
    [Header("Booster Used")]
    public bool isUsedX2Star;
    public bool isUsedTimeBuffer;
    public bool isUsedBoxBuffer;
    [Header("DAILY LOGIN STREAK")]
    public int playerClaimedDay;
    public bool hasClaimedStreakToday;
    [Header("DAILY TIERED REWARDS")]
    public bool isFreeClaimedToday;
    public int adRewardsClaimedCount;

    public DataPlayer()
    {
        isUsedX2Star = false;
        isUsedTimeBuffer = false;
        isUsedBoxBuffer = false;
        playerClaimedDay = 0;
        hasClaimedStreakToday = false;
        isFreeClaimedToday = false;
        adRewardsClaimedCount = 0;
    }

    public void Save()
    {
        JsonSaveSystem.Save(this, "PlayerData");
    }

    public static DataPlayer Load()
    {
        return JsonSaveSystem.Load<DataPlayer>("PlayerData");
    }
}