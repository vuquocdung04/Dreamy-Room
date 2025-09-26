
using Sirenix.OdinInspector;
using UnityEngine;

public class DataContains : MonoBehaviour
{
    public DataDailyBase dataDaily;
    public GiftDataBase giftData;
    public DataProfileBase dataProfile;
    public DataLevelBase dataLevel;
    public DataCollectionBase dataCollection;
    public DataBoosterBase dataBooster;
    public void Init()
    {
        dataLevel.Init();
        
        HasPassedDay();
    }

    private void HasPassedDay()
    {
        if (TimeManager.HasDayPassed(UseProfile.FirstTimeOpenGame, TimeManager.GetCurrentTime()))
        {
            dataDaily.PrepareForNewDay();
            UseProfile.FirstTimeOpenGame = TimeManager.GetCurrentTime();
        }
    }


    [Button("Buff Star 20", ButtonSizes.Large)]
    void BuffStar20()
    {
        UseProfile.Star += 20;
    }

    [Button("Buff Max Level", ButtonSizes.Large)]
    void BuffLevel(int maxLevel)
    {
        UseProfile.MaxUnlockedLevel = maxLevel;
    }
}
