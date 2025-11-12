
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
    public LocalizationData  localizationData;
    public AudioDataBase audioData;
    
    private DataPlayer dataPlayer;
    public DataPlayer DataPlayer => dataPlayer;
    public void Init()
    {
        LoadData();
        dataLevel.Init();
        HasPassedDay();
    }

    private void LoadData()
    {
        dataPlayer = JsonSaveSystem.Load<DataPlayer>("PlayerData");
        if (dataPlayer == null)
        {
            dataPlayer = new DataPlayer();
            SaveData(); 
        }
    }

    public void SaveData()
    {
        JsonSaveSystem.Save(dataPlayer, "PlayerData");
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
