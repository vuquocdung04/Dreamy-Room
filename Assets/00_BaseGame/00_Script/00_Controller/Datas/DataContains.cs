
using Sirenix.OdinInspector;
using UnityEngine;

public class DataContains : MonoBehaviour
{
    public DataDailySO dataDaily;
    public GiftDataBase giftData;
    public DataProfileSO dataProfile;
    public DataLevelSO dataLevel;
    public DataCollectionSO dataCollection;
    public void Init()
    {
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
}
