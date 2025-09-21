
using UnityEngine;

public class DataContains : MonoBehaviour
{
    public DataDailySO dataDaily;
    public GiftDataBase giftData;
    public void Init()
    {
        HasPassedDay();
    }

    public void HasPassedDay()
    {
        if (TimeManager.HasDayPassed(UseProfile.FirstTimeOpenGame, TimeManager.GetCurrentTime()))
        {
            dataDaily.ResetProgress();
            UseProfile.FirstTimeOpenGame = TimeManager.GetCurrentTime();
        }
    }
}
