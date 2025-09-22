
using UnityEngine;

public class DataContains : MonoBehaviour
{
    public DataDailySO dataDaily;
    public GiftDataBase giftData;
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
}
