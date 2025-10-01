using System;
using UnityEngine;

public class HeartGame : MonoBehaviour
{
    //[SerializeField] private long timeUpHeartGame = 30;
    //[SerializeField] private bool wasCoolDown;
    //[SerializeField] float currentCoolDown;

    public void Init()
    {
        //wasCoolDown = false;
        CheckHeart();
        CheckUnlimitHeart();
    }

    private void CheckUnlimitHeart()
    {
        if (UseProfile.IsUnlimitedHeart)
        {
            var temp = TimeManager.CalculateTime(DateTime.Now, UseProfile.TimeUnlimitedHeart);
            if (temp < 0)
            {
                UseProfile.IsUnlimitedHeart = false;
            }
        }
        var tempTime = TimeManager.CalculateTime(DateTime.Now, UseProfile.TimeUnlimitedHeart);
    }

    private void CheckHeart()
    {
        if (UseProfile.Heart < 5)
        {
            var secondsSinceLastUpdate = TimeManager.CalculateTime(UseProfile.TimeLastOverHeart, DateTime.Now);
            int minutesSinceLastUpdate = (int)secondsSinceLastUpdate / 60;

            if (minutesSinceLastUpdate >= 10)
            {
                
            }
        }
    }
}