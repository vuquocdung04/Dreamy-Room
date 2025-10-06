
using System;
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
        if (TimeManager.HasDayPassed(UseProfile.FirstTimeOpenGame, DateTime.Now))
        {
            dataDaily.PrepareForNewDay();
            UseProfile.FirstTimeOpenGame = DateTime.Now;
        }
    }
}
