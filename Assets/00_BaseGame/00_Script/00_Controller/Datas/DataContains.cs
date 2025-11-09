
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
    public DataPlayer dataPlayer;
    public LocalizationData  localizationData;
    public AudioDataBase audioData;
    public void Init()
    {
        dataLevel.Init();
        HasPassedDay();
    }

    private void HasPassedDay()
    {
        Debug.Log(TimeManager.HasDayPassed(UseProfile.TimeLastLoginDate, DateTime.Now));
        if (TimeManager.HasDayPassed(UseProfile.TimeLastLoginDate, DateTime.Now))
        {
            dataDaily.PrepareForNewDay();
            UseProfile.TimeLastLoginDate = DateTime.Now;
        }
    }
}
